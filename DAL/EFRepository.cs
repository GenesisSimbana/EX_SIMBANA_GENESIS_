using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class EFRepository : IRepository
    {
        DbContext _dbContext;
        public EFRepository(DbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public TEntity Create<TEntity>(TEntity ToCreate) where TEntity : class
        {
            TEntity result = null;
            try
            {
                _dbContext.Set<TEntity>().Add(ToCreate);
                _dbContext.SaveChanges();
                result = ToCreate;
            }
            catch (Exception)
            {
                throw;
            }
            return result;
        }

        public bool Delete<TEntity>(TEntity ToDelete) where TEntity : class
        {
            bool result = false;
            try
            {
                _dbContext.Entry<TEntity>(ToDelete).State = EntityState.Deleted;
                result = _dbContext.SaveChanges() > 0;
            }
            catch (Exception)
            {
                throw;
            }
            return result;
        }

        public void Dispose()
        {
            if (_dbContext != null)
            {
                _dbContext.Dispose();
            }
        }



        public List<TEntity> Filter<TEntity>(Expression<Func<TEntity, bool>> criteria) where TEntity : class
        {
            List<TEntity> result = null;
            try
            {
                result = _dbContext.Set<TEntity>().Where(criteria).ToList();
            }
            catch (Exception)
            {
                throw;
            }
            return result;
        }

        public TEntity Retrieve<TEntity>(Expression<Func<TEntity, bool>> criteria) where TEntity : class
        {
            TEntity result = null;
            try
            {
                result = _dbContext.Set<TEntity>().FirstOrDefault(criteria);
            }
            catch (Exception)
            {
                throw;
            }
            return result;
        }

        public bool Update<TEntity>(TEntity entity) where TEntity : class
        {
            try
            {
                // Recuperar la entidad existente usando la clave primaria.
                var primaryKey = entity.GetType().GetProperty("EstudianteID").GetValue(entity);
                var existingEntity = _dbContext.Set<TEntity>().Find(primaryKey);

                if (existingEntity != null)
                {
                    // Actualizar solo los valores necesarios en la entidad existente.
                    _dbContext.Entry(existingEntity).CurrentValues.SetValues(entity);
                    _dbContext.Entry(existingEntity).State = EntityState.Modified;
                }
                else
                {
                    // Adjuntar y marcar como modificada si la entidad no está rastreada.
                    _dbContext.Set<TEntity>().Attach(entity);
                    _dbContext.Entry(entity).State = EntityState.Modified;
                }

                // Guardar cambios.
                _dbContext.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                // Manejar excepciones y registrar errores si es necesario.
                throw new Exception("Error al actualizar la entidad.", ex);
            }
        }

    }
}
