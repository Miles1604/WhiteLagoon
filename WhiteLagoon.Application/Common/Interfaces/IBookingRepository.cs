using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using WhiteLagoon.Domain.Entities;

namespace WhiteLagoon.Application.Common.Interfaces
{
		public interface IBookingRepository : IRepository<Booking> //telling api we are implementing IRepository and pass through as class Villa
		{
	
			void Update(Booking entity);
		
		}
	}

