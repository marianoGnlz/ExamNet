namespace NetExam
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using NetExam.Abstractions;
    using NetExam.Dto;
    using NetExam.Models;

    public class OfficeRental : IOfficeRental
    {
        public IList<Location> Locations { get; set; } = new List<Location>();
        public IList<Office> Offices { get; set; } = new List<Office>();
        public IList<Booking> Bookings { get; set; } = new List<Booking>();

        public void AddLocation(LocationSpecs locationSpecs)
        {
            Location location = new Location { Name = locationSpecs.Name, Neighborhood = locationSpecs.Neighborhood };

            var locationExist = SearchLocation(location);

            if (locationExist)
            {
                throw new ArgumentException($"The location {locationSpecs.Name} already exists.");
            } 
            
            Locations.Add(location);
        }

        public void AddOffice(OfficeSpecs officeSpecs)
        {
            var locationExist = SearchLocation(new Location { Name = officeSpecs.LocationName });

            if (!locationExist)
            {
                throw new ArgumentException($"The Location '{officeSpecs.LocationName}' does not exist.");
            }

            Office office = new Office
            {
                LocationName = officeSpecs.LocationName,
                Name = officeSpecs.Name,
                Capacity = officeSpecs.MaxCapacity,
                Resourses = officeSpecs.AvailableResources
            };

            Offices.Add(office);
        }

        public void BookOffice(BookingRequest bookingRequest)
        {
            IOffice office = Offices.Where( o => o.LocationName == bookingRequest.LocationName &&
                                                 o.Name == bookingRequest.OfficeName
                                          ).SingleOrDefault();
            if (office == null)
            {
                throw new Exception($"The office {bookingRequest.OfficeName} in the location {bookingRequest.LocationName} does not exist.");
            }

            Booking booking = new Booking
            {
                DateTime = bookingRequest.DateTime,
                Office = office
            };

            var officeTaken = OfficeIsTaken(office);

            if(officeTaken)
            {
                throw new ArgumentException($"The office {office.Name} is already taken.");
            }

            Bookings.Add(booking);
        }

        public IEnumerable<IBooking> GetBookings(string locationName, string officeName)
        {
            IList<Booking> bookings = Bookings
                    .Where( b => b.Office.LocationName == locationName && 
                                 b.Office.Name == officeName
                    ).ToList();
            return bookings;
        }

        public IEnumerable<ILocation> GetLocations()
        {
            return Locations;
        }

        public IEnumerable<IOffice> GetOffices(string locationName)
        {
            IList<Office> offices = Offices.Where(o => o.LocationName == locationName)
                                            .ToList();
            return offices;
        }

        public IEnumerable<IOffice> GetOfficeSuggestion(SuggestionRequest suggestionRequest)
        {
            var (capacityNeeded, preferedNeigborHood, resourcesNeeded) = suggestionRequest;

            var idealOffices = Offices.Where(o => o.Capacity >= capacityNeeded &&
                                                  o.Resourses.Intersect(resourcesNeeded).Count() == resourcesNeeded.Count() &&
                                                  preferedNeigborHood == Locations.SingleOrDefault(l => l.Name == o.LocationName)
                                                                                  .Neighborhood      
                                            )
                                       .OrderBy(o => o.Capacity)
                                       .ThenBy(o => o.Resourses.Count());

            var suggestedOffices = Offices.Where(o => o.Capacity >= capacityNeeded &&
                                                       o.Resourses.Intersect(resourcesNeeded).Count() >= resourcesNeeded.Count() &&
                                                       preferedNeigborHood != Locations.SingleOrDefault(l => l.Name == o.LocationName)
                                                                                  .Neighborhood
                                                  )
                                        .OrderBy(o => o.Capacity)
                                        .ThenBy(o => o.Resourses.Count());
            var result = idealOffices.Union(suggestedOffices);

            return result;
        }

        private bool SearchLocation(ILocation location)
        {
            return Locations.Any(l => l.Name.ToLowerInvariant() == location.Name.ToLowerInvariant());
        }

        private bool OfficeIsTaken(IOffice office)
        {
            return Bookings.Any(b => b.Office.Name == office.Name);
        }
    }
}