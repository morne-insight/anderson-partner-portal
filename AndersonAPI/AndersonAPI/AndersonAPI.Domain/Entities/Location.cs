using Intent.RoslynWeaver.Attributes;

[assembly: IntentTemplate("Intent.Entities.DomainEntity", Version = "2.0")]

namespace AndersonAPI.Domain.Entities
{
    public class Location : BaseEntity
    {
        public Location(string name, Guid regionId, Guid countryId, bool isHeadOffice, Guid companyProfileId)
        {
            Name = name;
            RegionId = regionId;
            CountryId = countryId;
            IsHeadOffice = isHeadOffice;
            CompanyProfileId = companyProfileId;
        }
        /// <summary>
        /// Required by Entity Framework.
        /// </summary>
        protected Location()
        {
            Name = null!;
            Region = null!;
            Country = null!;
        }

        public string Name { get; private set; }

        public Guid RegionId { get; private set; }

        public Guid CountryId { get; private set; }

        public bool IsHeadOffice { get; private set; }

        public Guid CompanyProfileId { get; private set; }

        public virtual Region Region { get; private set; }

        public virtual Country Country { get; private set; }

        public void Update(string name, bool isHeadOffice, Guid regionId, Guid countryId, Guid companyProfileId)
        {
            Name = name;
            IsHeadOffice = isHeadOffice;
            RegionId = regionId;
            CountryId = countryId;
            CompanyProfileId = companyProfileId;
        }
    }
}