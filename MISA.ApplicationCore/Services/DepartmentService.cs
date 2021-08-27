using MISA.ApplicationCore.Entities;
using MISA.ApplicationCore.Interfaces.Repositories;
using MISA.ApplicationCore.Interfaces.Services;
using MISA.Entity.MISA.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.ApplicationCore.Services
{
    public class DepartmentService : BaseService<Department>, IDepartmentService
    {
        private readonly IDepartmentRepository _departmentRepository;
        private readonly ServiceResponse _serviceResponse;

        public DepartmentService(IBaseRepository<Department> baseRepository,
            IDepartmentRepository departmentRepository) : base(baseRepository)
        {
            _departmentRepository = departmentRepository;
            _serviceResponse = new ServiceResponse();
        }
    }
}
