using ServiceContracts.DTO;

namespace ServiceContracts;

public interface IPersonsPdfGenerator
{
    public interface IPersonPdfGenerator
    {
        byte[] GeneratePersonsPdf(List<PersonResponse> persons);
    }

}