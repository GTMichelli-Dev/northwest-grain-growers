

#nullable enable
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Data;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using Seed25.DTO;





[ApiController]
[Route("api/[controller]")]
public class ScaleController : ControllerBase
{
    private const string ServiceUrl = "http://WASDB07366LB/localwebservice.asmx";
    private static readonly XNamespace NsSoap11 = "http://schemas.xmlsoap.org/soap/envelope/";
    private static readonly XNamespace NsSoap12 = "http://www.w3.org/2003/05/soap-envelope";
    private static readonly XNamespace NsLocal = "http://nwgglocal/";
    
    private static List<ScaleDTO> ScaleList = new List<ScaleDTO>();

    [HttpPost("UpdateScale")]
    public  IActionResult UpdateScale([FromBody] ScaleDTO scale)
    {
        if (scale == null || string.IsNullOrWhiteSpace(scale.Description))
        {
            return BadRequest(new { error = "Invalid scale data." });
        }

        var existingScale = ScaleList.Find(s => s.Description.Equals(scale.Description, StringComparison.OrdinalIgnoreCase));
        if (existingScale == null)
        {
            ScaleList.Add(scale);
        }
        else
        {
            existingScale.Motion = scale.Motion;
            existingScale.Ok = scale.Ok;
            existingScale.Weight = scale.Weight;
            existingScale.Status = scale.Status;
            existingScale.LastUpdate = scale.LastUpdate;
        }
        return Ok(new { message = "Scale updated successfully." });
    }

    [HttpGet("GetScales")]
    public async Task<IActionResult> GetScales()
    {
        // 1) Build SOAP 1.1 envelope (ASMX usually expects SOAP 1.1 + SOAPAction)
        var envelope = @"<?xml version=""1.0"" encoding=""utf-8""?>
<soap:Envelope xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance""
               xmlns:xsd=""http://www.w3.org/2001/XMLSchema""
               xmlns:soap=""http://schemas.xmlsoap.org/soap/envelope/"">
  <soap:Body>
    <GetScales xmlns=""http://nwgglocal/"" />
  </soap:Body>
</soap:Envelope>";

        // 2) Use default Windows credentials if this is intranet-protected
        var handler = new HttpClientHandler { UseDefaultCredentials = true, AllowAutoRedirect = true };
        using var http = new HttpClient(handler) { Timeout = TimeSpan.FromSeconds(30) };

        using var req = new HttpRequestMessage(HttpMethod.Post, ServiceUrl);
        req.Headers.Add("SOAPAction", "\"http://nwgglocal/GetScales\"");
        req.Content = new StringContent(envelope, Encoding.UTF8, "text/xml");

        using var resp = await http.SendAsync(req, HttpCompletionOption.ResponseContentRead);
        var payload = await resp.Content.ReadAsStringAsync();

        if (!resp.IsSuccessStatusCode)
        {
            // If it’s a 401/403/500 HTML page, surface that clearly
            return StatusCode((int)resp.StatusCode, new
            {
                error = "Upstream call failed",
                status = resp.StatusCode.ToString(),
                preview = payload[..Math.Min(payload.Length, 500)]
            });
        }

        // 3) Try SOAP 1.1 first, then fallback to SOAP 1.2, else detect plain XML
        string? innerXml = null;
        XDocument? doc = null;

        try
        {
            doc = XDocument.Parse(payload);
        }
        catch
        {
            // Not XML at all (likely HTML or empty) -> return helpful preview
            return BadRequest(new
            {
                error = "Service did not return XML.",
                preview = payload[..Math.Min(payload.Length, 500)]
            });
        }

        // Try to locate GetScalesResult in SOAP 1.1
        var resultNode = doc.Root?
            .Element(NsSoap11 + "Body")?
            .Element(NsLocal + "GetScalesResponse")?
            .Element(NsLocal + "GetScalesResult");

        // If not found, try SOAP 1.2
        resultNode ??= doc.Root?
            .Element(NsSoap12 + "Body")?
            .Element(NsLocal + "GetScalesResponse")?
            .Element(NsLocal + "GetScalesResult");

        // If still not found, maybe it’s a SOAP Fault or a plain XML DataSet
        if (resultNode == null)
        {
            // SOAP Fault?
            var fault11 = doc.Root?.Element(NsSoap11 + "Body")?.Element(NsSoap11 + "Fault");
            var fault12 = doc.Root?.Element(NsSoap12 + "Body")?.Element(NsSoap12 + "Fault");
            if (fault11 != null || fault12 != null)
            {
                return Problem(detail: (fault11 ?? fault12)!.ToString(), title: "SOAP Fault from ASMX", statusCode: 502);
            }

            // Plain dataset XML?
            // e.g. <Weigh_ScalesDataTable ...>...</Weigh_ScalesDataTable>
            try
            {
                var table = ReadDataTableFromXml(doc.CreateReader());
        
                    
            }
            catch (Exception exPlain)
            {
                return BadRequest(new
                {
                    error = "Could not find SOAP result nor parse plain XML.",
                    exception = exPlain.Message,
                    preview = payload[..Math.Min(payload.Length, 500)]
                });
            }
        }

        // 4) We have GetScalesResult. Two cases:
        //    A) Inner content is real XML as *child elements*
        //    B) Inner content is an XML string (escaped text) like "&lt;Weigh_ScalesDataTable..."
        DataTable? tableParsed;

        if (resultNode != null && resultNode.HasElements)
        {
            // A) Elements present -> read directly from the node
            tableParsed = ReadDataTableFromXml(resultNode.CreateReader());
        }
        else if (resultNode != null)
        {
            // B) Text content -> try to parse the inner text as XML
            innerXml = (resultNode.Value ?? string.Empty).Trim();

            if (string.IsNullOrWhiteSpace(innerXml))
                return Ok(Array.Empty<object>());

            if (!innerXml.StartsWith("<"))
            {
                // Not XML at the root -> likely HTML-escaped or an error string
                return BadRequest(new
                {
                    error = "GetScalesResult contained non-XML text.",
                    preview = innerXml[..Math.Min(innerXml.Length, 500)]
                });
            }

            // Parse the inner XML string and read as DataSet
            var innerDoc = XDocument.Parse(innerXml);
            tableParsed = ReadDataTableFromXml(innerDoc.CreateReader());
        }
        else
        {
            // resultNode is null, handle accordingly if needed
            return Ok(Array.Empty<object>());
        }
        var RawData = ToJsonRows(tableParsed);
        List<ScaleDTO> scales = new List<ScaleDTO>();
        foreach (var row in RawData)
        {
            var ok = true;
            if (row.ContainsKey("UID"))
            {
                var scale = new ScaleDTO();
                var uidObj = row["UID"];
               
               

                var descriptionObj = row["Description"];
                if (descriptionObj != null)
                {
                    scale.Description = descriptionObj as string ?? descriptionObj?.ToString() ?? string.Empty;
                }
                else
                {
                    ok = false;
                }

                var motionObj = row["Motion"];
                if (motionObj != null && bool.TryParse(motionObj.ToString(), out var motionValue))
                {
                    scale.Motion = motionValue;
                }
                else
                {
                    ok = false;
                }

                var okObj = row["OK"];
                if (okObj != null && bool.TryParse(okObj.ToString(), out var okValue))
                {
                    scale.Ok = okValue;
                }
                else
                {
                    ok = false;
                }

                var weightObj = row["Weight"];
                if (weightObj != null && int.TryParse(weightObj.ToString(), out var weightValue))
                {
                    scale.Weight = weightValue;
                }
                else
                {
                    ok = false;
                }

                var statusObj = row["Status"];
                if (statusObj != null)
                {
                    scale.Status = statusObj as string ?? statusObj?.ToString() ?? string.Empty;
                }
                else
                {
                    ok = false;
                }

               

                var updateObj = row["Last_Update"];
                if (updateObj != null && DateTime.TryParse(updateObj.ToString(), out var LastUpdateValue))
                {
                    scale.LastUpdate = LastUpdateValue;
                }
                else
                {
                    ok = false;
                }
                if (ok) scales.Add(scale);

            }
        }
        return Ok(scales);
    }

    private static DataTable ReadDataTableFromXml(XmlReader xmlReader)
    {
        var ds = new DataSet();
        ds.ReadXml(xmlReader, XmlReadMode.Auto); // handles diffgram + regular
        if (ds.Tables.Count == 0)
            return new DataTable(); // empty
        // If multiple tables are returned, pick the one named like your sample
        var table = ds.Tables.Contains("Weigh_ScalesDataTable") ? ds.Tables["Weigh_ScalesDataTable"]! : ds.Tables[0];
        return table;
    }

    private static List<Dictionary<string, object?>> ToJsonRows(DataTable table)
    {
        var rows = new List<Dictionary<string, object?>>(table.Rows.Count);
        foreach (DataRow r in table.Rows)
        {
            var dict = new Dictionary<string, object?>(table.Columns.Count, StringComparer.OrdinalIgnoreCase);
            foreach (DataColumn c in table.Columns)
                dict[c.ColumnName] = r[c];
            rows.Add(dict);
        }
        return rows;
    }
}
