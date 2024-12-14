using System;
using System.Collections.Generic;
using System.Web.Http;
using Entities;
using BLL;
using SLC;
using System.Net;

namespace Service.Controllers
{
    [RoutePrefix("api/Estudiantes")]
    public class EstudiantesController : ApiController
    {
        private readonly StudentsLogic _studentsLogic;

        public EstudiantesController()
        {
            // Instancia de la lógica de estudiantes
            _studentsLogic = new StudentsLogic();
        }

        // GET: api/Estudiantes
        [HttpGet]
        [Route("")]
        public IHttpActionResult GetAll()
        {
            try
            {
                var estudiantes = _studentsLogic.RetrieveAll();
                return Ok(estudiantes);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        // GET: api/Estudiantes/{id}
        [HttpGet]
        [Route("{id:int}")]
        public IHttpActionResult GetById(int id)
        {
            try
            {
                var estudiante = _studentsLogic.RetrieveById(id);
                if (estudiante == null)
                {
                    return NotFound();
                }
                return Ok(estudiante);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        // POST: api/Estudiantes
        [HttpPost]
        [Route("")]
        public IHttpActionResult Create([FromBody] Estudiantes estudiante)
        {
            if (estudiante == null)
            {
                return BadRequest("El objeto estudiante no puede ser nulo.");
            }

            try
            {
                var createdStudent = _studentsLogic.Create(estudiante);
                return Created($"api/Estudiantes/{createdStudent.EstudianteID}", createdStudent);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        // PUT: api/Estudiantes/{id}
        [HttpPut]
        [Route("{id:int}")]
        public IHttpActionResult Update(int id, [FromBody] Estudiantes estudiante)
        {
            if (estudiante == null || estudiante.EstudianteID != id)
            {
                return BadRequest("La información del estudiante es inconsistente.");
            }

            try
            {
                var updated = _studentsLogic.Update(estudiante);
                if (updated)
                {
                    return Ok(new { message = "Estudiante actualizado exitosamente." });
                }
                else
                {
                    return NotFound();
                }
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }


        // DELETE: api/Estudiantes/{id}
        [HttpDelete]
        [Route("{id:int}")]
        public IHttpActionResult Delete(int id)
        {
            try
            {
                var deleted = _studentsLogic.Delete(id);
                if (!deleted)
                {
                    return Content(HttpStatusCode.NotFound, new { message = "Estudiante no encontrado." });
                }

                return Content(HttpStatusCode.OK, new { message = "El estudiante fue eliminado correctamente." });
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.InternalServerError, new { message = ex.Message });
            }
        }

    }
}
