using FluentValidation;

namespace vue_businesslogic.Models
{
    public class PostToWebScrapeRequest
    {
        public string Url { get; set; } = String.Empty;

        public string Keywords { get; set; } = String.Empty;
    }

    public class PostToWebScrapeRequestValidator : AbstractValidator<PostToWebScrapeRequest>
    {
        public PostToWebScrapeRequestValidator()
        {
            RuleFor(x => x.Url).Must(IsUrlValid).WithMessage("Please input a valid url");
        }

        protected bool IsUrlValid(string url)
        {
            if (string.IsNullOrEmpty(url))
            {
                return false;
            }

            return Uri.TryCreate(url, UriKind.Absolute, out Uri output) &&
                (output.Scheme == Uri.UriSchemeHttp || output.Scheme == Uri.UriSchemeHttps);
        } 
    }
}
