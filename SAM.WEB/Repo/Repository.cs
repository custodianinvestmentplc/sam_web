using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection.Metadata;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.AspNetCore.Http;
using SAM.WEB.Domain.Dtos;

namespace SAM.WEB.Repo
{
    public static class Repository<T> where T : class
    {
        public static void Add(T entity, string _key)
        {
            RoutesController<T>.PostDbSet(entity, _key);
        }

        //public static void Update(T obj, string _key, string id)
        //{
        //    RoutesController<T>.UpdateDbSet(obj, _key, id);
        //}

        public static T Find(Predicate<T> filter, string _key)
        {
            var dbSet = RoutesController<T>.GetDbSet(_key);

            var matched = dbSet.Find(filter);

            return matched;
        }

        public static List<T> GetAll(string _key, Expression<Func<T, bool>> filter = null)
        {
            var dbSet = RoutesController<T>.GetDbSet(_key);

            IQueryable<T> query = dbSet.AsQueryable();

            if (filter != null)
            {
                query = query.Where(filter);
            }

            return query.ToList();
        }

        //public static void Remove(string _key, string id)
        //{
        //    //dbSet.Remove(entity);
        //    RoutesController<T>.DeleteDbSet(id, _key);
        //}
    }
}