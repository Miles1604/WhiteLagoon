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
	public class ApplicationUserRepository : Repository<ApplicationUser>, IApplicationUserRepository // this is saying villa repository will get all methods from generic repository
                                                                                                     // and is implementing the IVillaRepository

    {
		private readonly ApplicationDbContext _db;
		public ApplicationUserRepository(ApplicationDbContext db) : base(db)
		{
			_db = db;
		}
		
	}
}
