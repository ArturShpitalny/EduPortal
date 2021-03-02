using EducationPortal.Core.Models.Entities;
using EducationPortal.Core.Models.Entities.Base;
using EducationPortal.Core.Models.States;
using EducationPortal.Core.Models.DTO;
using EducationPortal.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EducationPortal.BLL.Services
{
    public class MaterialService
    {
        private readonly IRepository repository;        

        public MaterialService(IRepository repository)
        {
            this.repository = repository;            
        }

        //Adding New Material
        public ResponseState AddMaterial<TMaterial>(TMaterial material) where TMaterial : Material
        {
            this.repository.Create<TMaterial>(material);
            this.repository.SaveChanges();

            return new ResponseState { State = true, Massage = $"OK" };
        }

        //The output of all materials with a split
        public EntityItemModel<Material> GetMaterials(int elementOnPageCount, int page, string search)
        {
            int countTotalItems = this.repository.Count<Material>(x => x.MaterialName.ToUpper().Contains(search.ToUpper()) || x.MaterialDescription.ToUpper().Contains(search.ToUpper()));

            IEnumerable<Material> paginationMaterials = this.repository.GetDataBlock<Material, int>((page - 1) * elementOnPageCount, elementOnPageCount, o => o.Id, x => x.MaterialName.ToUpper().Contains(search.ToUpper()) || x.MaterialDescription.ToUpper().Contains(search.ToUpper())).ToList();

            Pagination pages = new Pagination
            {
                PageNumber = page,
                PageSize = elementOnPageCount,
                TotalItems = countTotalItems
            };

            return new EntityItemModel<Material> { Entities = paginationMaterials, Pagination = pages };
        }

        //Display material object by ID
        public TMaterial GetMaterial<TMaterial>(int materialId) where TMaterial : Material
        {
            return this.repository.FirstOrDefault<TMaterial>(m => m.Id == materialId);
        }

        //Material editing
        public bool UpdateMaterial(Article article)
        {
            Article entity = this.repository.FirstOrDefault<Article>(x => x.Id == article.Id);

            if(entity != null)
            {
                entity.MaterialName = article.MaterialName;
                entity.MaterialDescription = article.MaterialDescription;
                entity.MaterialResource = article.MaterialResource;
                entity.SkillId = article.SkillId;
                entity.ArticlePublicationDate = article.ArticlePublicationDate;                

                this.repository.Update<Article>(entity);
                this.repository.SaveChanges();

                return true;
            }
            return false;            
        }

        public bool UpdateMaterial(Book book)
        {
            Book entity = this.repository.FirstOrDefault<Book>(x => x.Id == book.Id);

            if (entity != null)
            {
                entity.MaterialName = book.MaterialName;
                entity.MaterialDescription = book.MaterialDescription;
                entity.MaterialResource = book.MaterialResource;
                entity.SkillId = book.SkillId;
                entity.BookAuthors = book.BookAuthors;
                entity.BookFormat = book.BookFormat;
                entity.BookPages = book.BookPages;
                entity.BookYear = book.BookYear;                

                this.repository.Update<Book>(entity);
                this.repository.SaveChanges();

                return true;
            }
            return false;
        }

        public bool UpdateMaterial(Video video)
        {
            Video entity = this.repository.FirstOrDefault<Video>(x => x.Id == video.Id);

            if (entity != null)
            {
                entity.MaterialName = video.MaterialName;
                entity.MaterialDescription = video.MaterialDescription;
                entity.MaterialResource = video.MaterialResource;
                entity.SkillId = video.SkillId;
                entity.VideoLength = video.VideoLength;
                entity.VideoResolution = video.VideoResolution;

                this.repository.Update<Video>(entity);
                this.repository.SaveChanges();

                return true;
            }
            return false;
        }

        //Delete material by ID
        public ResponseState DeleteMaterial(int materialId)
        {
            Material material = this.repository.FirstOrDefault<Material>(x => x.Id == materialId);

            if (material != null)
            {
                this.repository.Delete<Material>(material);
                this.repository.SaveChanges();

                return new ResponseState { State = true, Massage = "OK" };
            }

            return new ResponseState { State = false, Massage = "MaterialIsAbsent" };
        }
    }
}
