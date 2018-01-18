using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
namespace HDD_Calculator
{
    class CameraContext : DbContext
    {
        public DbSet<Camera> Cameras { get; set; }
    }
}
