namespace Crm.Api.Agent.Security
{
    public sealed class AgentAuthOptions
    {
        // Neden: İlk kayıt (register) sırasında agent’ın sisteme yetkili şekilde eklenmesi için paylaşılan anahtar.
        // Bu anahtar sadece devops/admin tarafından bilinir ve agent kurulumunda konfigüre edilir.
        public string RegistrationKey { get; set; } = "CHANGE_ME_REGISTER_SECRET";
    }
}
