using Microsoft.EntityFrameworkCore;
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
	public class AmenityRepository : Repository<Amenity>, IAmenityRepository // this is saying villa repository will get all methods from generic repository
																			 // and is implementing the IVillaRepository

	{
		private readonly ApplicationDbContext _db;
		public AmenityRepository(ApplicationDbContext db) : base(db)
		{
			_db = db;
		}
		public void Add(Amenity entity)
		{
			_db.Add(entity);
		}

		

		public void Update(Amenity entity)
		{
			_db.Amenities.Update(entity);
		}
	}
}
