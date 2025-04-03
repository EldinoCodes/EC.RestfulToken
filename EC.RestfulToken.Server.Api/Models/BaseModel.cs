namespace EC.RestfulToken.Server.Api.Models;

/*
 * always nice to have a base model to handle common properties, 
 * but be careful as you can go overboard and limit the flexibility 
 * of your models!!
 */

public abstract class BaseModel
{
    public DateTime? CreatedDate { get; set; }
    public string? CreatedBy { get; set; }
    public DateTime? ModifiedDate { get; set; }
    public string? ModifiedBy { get; set; }
}
