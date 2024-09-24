using MessageHub.Domain.DTO;
using MessageHub.Domain.Entities;
using MessageHub.Domain.Exceptions;
using MessageHub.Infrastructure.Repositories;
using MessageHub.Services.Base;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace MessageHub.Services
{
    /// <summary>
    /// Service for downloading templates from a remote server
    /// </summary>
    public class TemplateDownloadService(ILogger<TemplateDownloadService> logger, UnitOfWork unitOfWork) : BaseService(logger, unitOfWork)
    {
        private readonly HttpClient _httpClient = new HttpClient();

        public async Task<int> DownloadTemplatesAsync(string url)
        {
            int resultCount = 0;
            Logger.LogInformation("Downloading templates list from {url}", url);
            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();

            Logger.LogInformation("Downloaded templates list from {url}", url);
            var json = await response.Content.ReadAsStringAsync();
            var templateList = JsonConvert.DeserializeObject<TemplateList>(json) ?? throw new NotFoundException("Template list not found");

            foreach (var templateInfo in templateList.Templates)
            {
                try
                {
                    Logger.LogDebug("Downloading template {code} from {url}", templateInfo.Code, templateInfo.Url);
                    var templateContent = await _httpClient.GetStringAsync(templateInfo.Url);
                    Logger.LogDebug("Template downloaded successfully {code}", templateInfo.Code);

                    Template? template = await UnitOfWork.TemplateRepository.GetTemplateByCodeAsync(templateInfo.Code);
                    if (template == null)
                    {
                        template = new Template();
                        await UnitOfWork.TemplateRepository.InsertAsync(template);
                    }

                    template.Type = templateInfo.Type;
                    template.Name = templateInfo.Name;
                    template.Code = templateInfo.Code;
                    template.Url = templateInfo.Url;
                    template.Content = templateContent;
                    template.Subject = templateInfo.Subject;
                    template.DateMod = DateTime.UtcNow;
                    resultCount++;
                }
                catch (Exception ex)
                {
                    Logger.LogError(ex, "Error in processing template {code} from {url}", templateInfo.Code, templateInfo.Url);
                }
            }

            await UnitOfWork.SaveChangesAsync();
            Logger.LogInformation("Downloaded {count} templates from {url}", resultCount, url);
            return resultCount;
        }
    }
}
