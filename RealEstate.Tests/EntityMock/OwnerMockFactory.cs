using RealEstate.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstate.Tests.EntityMock
{
    public static class OwnerMockFactory
    {
        public static List<Owner> CreateOwners(int count)
        {
            if (count <= 0)
                throw new ArgumentException("Count must be greater than zero.", nameof(count));

            var owners = new List<Owner>();
            for (int i = 0; i < count; i++)
            {
                owners.Add(new Owner
                {
                    Id = i + 1, // Assuming Ids start from 1
                    Name = $"Owner {i + 1}",
                    Address = $"Owner Address {i + 1}",
                    Photo = $"http://example.com/photo{i + 1}.jpg",
                    Birthday = DateTime.UtcNow.AddYears(-30).AddDays(i) // Example birthday
                });
            }

            return owners;
        }
    }

}
