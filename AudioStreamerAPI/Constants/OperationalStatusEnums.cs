namespace AudioStreamerAPI.Constants
{
    public enum OperationalStatusEnums : uint
    {
        Unimplemented = 0,
        Ok = 200,
        Created = 201,
        NoContent = 204,
        PartialContent = 206,
        BadRequest = 400,
        Unauthorized = 401,
        NotFound = 404,
        Conflict = 409,
    }
}
