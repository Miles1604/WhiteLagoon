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
	public class VillaRepository : Repository<Villa>, IVillaRepository // this is saying villa repository will get all methods from generic repository
																	   // and is implementing the IVillaRepository

	{
		private readonly ApplicationDbContext _db;
		public VillaRepository(ApplicationDbContext db) : base(db)
		{
			_db = db;
		}
		public void Add(Villa entity)
		{
			_db.Add(entity);
		}

		

		public void Save()
		{
			_db.SaveChanges();
		}

		public void Update(Villa entity)
		{
			_db.Villas.Update(entity);
		}
	}
}
