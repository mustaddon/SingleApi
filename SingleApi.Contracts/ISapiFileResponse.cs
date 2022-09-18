namespace SingleApi
{

    public interface ISapiFileResponse : ISapiFile
    {
        SapiFileDispositions Disposition { get; set; }
    }
}
