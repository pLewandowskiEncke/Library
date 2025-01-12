using FluentNHibernate.Mapping;
using Library.Domain.Entities;

namespace Library.Infrastructure.Data.Mappings
{
    public class BookMap : ClassMap<Book>
    {
        public BookMap()
        {
            Table("Books");
            Id(x => x.Id).GeneratedBy.Identity();
            Map(x => x.Title).Not.Nullable().Length(50);
            Map(x => x.Author).Not.Nullable().Length(50); 
            Map(x => x.ISBN).Not.Nullable().Unique().Length(20); 
            Map(x => x.Status).CustomType<int>().Not.Nullable();
        }
    }
}
