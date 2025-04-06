using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using WhiteLagoon.Application.Common.Interfaces;
using WhiteLagoon.Application.Common.Utility;
using WhiteLagoon.Domain.Entities;
using WhiteLagoon.Infrastructure.Data;
using static System.Net.Mime.MediaTypeNames;


namespace WhiteLagoon.Infrastructure.Repository
	
{
	public class BookingRepository : Repository<Booking>, IBookingRepository

	{
		private readonly ApplicationDbContext _db;
		public BookingRepository(ApplicationDbContext db) : base(db)
		{
			_db = db;
		}
		public void Add(Booking entity)
		{
			_db.Add(entity);
		}

		

		public void Update(Booking entity)
		{
			_db.Bookings.Update(entity);
		}

        public void UpdateStatus(int bookingId, string bookingStatus)
        {
            var bookingFromDb = _db.Bookings.FirstOrDefault(m => m.Id == bookingId);
			if (bookingFromDb != null) 
			{
				bookingFromDb.Status = bookingStatus;
				if(bookingStatus == SD.StatusCheckedIn)
				{
					bookingFromDb.ActualCheckInDate = DateTime.Now;
				}
                if (bookingStatus == SD.StatusCompleted)
                {
                    bookingFromDb.ActualCheckOutDate = DateTime.Now;
                }
            }
        }

        public void UpdateStripePaymentID(int bookingId, string sessionId, string paymentIntentId)
        {
            var bookingFromDb = _db.Bookings.FirstOrDefault(m => m.Id == bookingId);
            if (bookingFromDb != null)
			{
				if(!string.IsNullOrEmpty(sessionId)) //if sessionid is  null or empty we will update the booking from db with session id
				{ 
				bookingFromDb.StripeSessionId = sessionId;
				}
                if (!string.IsNullOrEmpty(paymentIntentId)) //if sessionid is  null or empty we will update the booking from db with session id
                {
                    bookingFromDb.StripePaymentIntentId = paymentIntentId;
					bookingFromDb.PaymentDate = DateTime.Now;
					bookingFromDb.IsPaymentSuccessful = true; //when we get paymentid from stripe this means payment is true so we log this to db
                }
            }
        }
    }
}
