using DemoApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DemoApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly Prn232Slot2Context _con;
        public StudentController(Prn232Slot2Context con)
        {
            _con = con;
        }
        [HttpGet]
        public async Task<IActionResult> Get() {
            var obj = await _con.Students.ToListAsync();
            return Ok(obj);
        }

        [HttpPost]
        public async Task<IActionResult> doPost([FromBody] StudentO st)
        {
            if (st == null) return BadRequest("Account not found");
            var obj = _con.Students.Find(st.Id);
            if (obj != null) return Conflict("Duplicate Id");
            _con.Students.Add(st.ToStudent());
            _con.SaveChanges();
            return Ok(obj);
        }
        [HttpPut("/id")]
        public  async Task<IActionResult> update(int id, StudentO st)
        {
            var obj = _con.Students.Find(id);
            if (obj == null) return BadRequest("Student is not found");
            StudentO.swap(st, obj);
            _con.SaveChanges();
            return Ok(obj);

        }
        [HttpDelete]
        public async Task<IActionResult> delete(int id)
        {
            var obj = _con.Students.Find(id);
            if (obj == null) return NotFound("Student is not found");
            _con.Students.Remove(obj);
            _con.SaveChanges();
            return Ok("da xoa thanh cong");
        }
    }
}
