﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using WhiteLagoon.Application.Common.Interfaces;
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
	}
}
