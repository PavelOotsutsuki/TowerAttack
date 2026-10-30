using UnityEngine.Networking;

namespace Servers
{
    public class BypassCertificate : CertificateHandler
    {
        protected override bool ValidateCertificate(byte[] certificateData) => true;
    }
}