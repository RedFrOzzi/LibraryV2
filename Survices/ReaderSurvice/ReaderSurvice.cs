using System.Security.Claims;

namespace LibraryV2.Survices.ReaderSurvice
{
    public class ReaderSurvice : IReaderSurvice
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ReaderSurvice(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string GetReaderName()
        {
            if (_httpContextAccessor.HttpContext != null)
                return _httpContextAccessor.HttpContext.User?.FindFirstValue(ClaimTypes.Name) ?? string.Empty;

            return string.Empty;
        }

        public string GetReaderId()
        {
            if (_httpContextAccessor.HttpContext != null)
                return _httpContextAccessor.HttpContext.User?.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;

            return string.Empty;
        }

        public string GetReaderRole()
        {
            if (_httpContextAccessor.HttpContext != null)
                return _httpContextAccessor.HttpContext.User?.FindFirstValue(ClaimTypes.Role) ?? string.Empty;

            return string.Empty;
        }
    }
}
