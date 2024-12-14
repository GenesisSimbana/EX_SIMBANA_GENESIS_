using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Entities;
using DAL;

namespace BLL
{
    public class StudentsLogic
    {
        public Estudiantes Create(Estudiantes student)
        {
            if (student == null)
            {
                throw new ArgumentException("El objeto estudiante no puede ser nulo.");
            }

            ValidateStudent(student);

            Estudiantes _student = null;
            using (var repository = RepositoryFactory.CreateRepository())
            {
                Estudiantes _result = repository.Retrieve<Estudiantes>
                    (s => s.Email == student.Email);
                if (_result == null)
                {
                    _student = repository.Create(student);
                }
                else
                {
                    throw new Exception("Estudiante con este correo ya existe.");
                }
            }
            return _student;
        }

        public Estudiantes RetrieveById(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("El ID debe ser un número positivo.");
            }

            Estudiantes _student = null;
            using (var repository = RepositoryFactory.CreateRepository())
            {
                _student = repository.Retrieve<Estudiantes>(s => s.EstudianteID == id);
            }
            return _student;
        }

        public bool Update(Estudiantes student)
        {
            if (student == null)
            {
                throw new ArgumentException("El objeto estudiante no puede ser nulo.");
            }

            ValidateStudent(student);

            bool _updated = false;
            using (var repository = RepositoryFactory.CreateRepository())
            {
                Estudiantes _result = repository.Retrieve<Estudiantes>
                    (s => s.EstudianteID == student.EstudianteID);
                if (_result != null)
                {
                    _updated = repository.Update(student);
                }
                else
                {
                    throw new Exception("Estudiante no existe.");
                }
            }
            return _updated;
        }

        public bool Delete(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("El ID debe ser un número positivo.");
            }

            var _student = RetrieveById(id);

            if (_student == null)
            {
                throw new Exception("Estudiante no encontrado.");
            }

            bool _delete = false;
            using (var repository = RepositoryFactory.CreateRepository())
            {
                _delete = repository.Delete(_student);
            }

            return _delete;
        }

        public List<Estudiantes> RetrieveAll()
        {
            using (var repository = RepositoryFactory.CreateRepository())
            {
                return repository.Filter<Estudiantes>(s => s.EstudianteID > 0).ToList();
            }
        }

        private void ValidateStudent(Estudiantes student)
        {
            // Validar valores nulos o vacíos
            if (string.IsNullOrWhiteSpace(student.Nombre) ||
                string.IsNullOrWhiteSpace(student.Apellido) ||
                string.IsNullOrWhiteSpace(student.Email))
            {
                throw new ArgumentException("El nombre, apellido y correo electrónico son obligatorios.");
            }

            // Validar caracteres especiales en el nombre y apellido
            if (!Regex.IsMatch(student.Nombre, @"^[a-zA-Z\s]+$") ||
                !Regex.IsMatch(student.Apellido, @"^[a-zA-Z\s]+$"))
            {
                throw new ArgumentException("El nombre y el apellido solo deben contener letras.");
            }

            // Validar formato de correo electrónico
            if (!Regex.IsMatch(student.Email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            {
                throw new ArgumentException("El formato del correo electrónico no es válido.");
            }
        }
    }
}
