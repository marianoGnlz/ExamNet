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
        public IList<ILocation> Locations { get; set; } = new List<ILocation>();
        public IList<IOffice> Offices { get; set; } = new List<IOffice>();
        public IList<Booking> Bookings { get; set; } = new List<Booking>();

        public void AddLocation(LocationSpecs locationSpecs)
        {
            ILocation location = new Location { Name = locationSpecs.Name };

            Locations.Add(location);
        }

        public void AddOffice(OfficeSpecs officeSpecs)
        {
            IOffice office = new Office
            {
                LocationName = officeSpecs.LocationName,
                Name = officeSpecs.Name
            };

            Offices.Add(office);
        }

        public void BookOffice(BookingRequest bookingRequest)
        {
            IOffice office = new Office
            {
                LocationName = bookingRequest.LocationName,
                Name = bookingRequest.OfficeName
            };
            Booking booking = new Booking
            {
                DateTime = bookingRequest.DateTime,
                Office = office
            };

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
            IList<IOffice> offices = Offices.Where(o => o.LocationName == locationName)
                                .ToList();
            return offices;
        }

        public IEnumerable<IOffice> GetOfficeSuggestion(SuggestionRequest suggestionRequest)
        {
            throw new NotImplementedException();
        }
    }
}