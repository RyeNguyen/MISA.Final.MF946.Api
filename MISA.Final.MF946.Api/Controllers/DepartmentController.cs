using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MISA.ApplicationCore.Interfaces.Repositories;
using MISA.ApplicationCore.Interfaces.Services;
using MISA.Entity.MISA.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MISA.MF946.Final.Api.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class DepartmentsController : BaseEntityController<Department>
    {
        #region Declares
        private readonly IDepartmentService _departmentService;
        private readonly IDepartmentRepository _departmentRepository;
        #endregion

        #region Constructor
        public DepartmentsController(IDepartmentService departmentService,
            IDepartmentRepository departmentRepository) : base(departmentService, departmentRepository)
        {
            _departmentService = departmentService;
            _departmentRepository = departmentRepository;
        }
        #endregion
    }
}
