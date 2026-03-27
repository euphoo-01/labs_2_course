namespace DAL_Celebrity;

public class Celebrity {
    public Celebrity() { 
        this.FullName = string.Empty; 
        this.Nationality = string.Empty; 
    }
    public int Id { get; set; }
    public string FullName { get; set; }
    public string Nationality { get; set; } 
    public string? ReqPhotoPath { get; set; }
    public virtual bool Update(Celebrity celebrity) {
        this.FullName = celebrity.FullName;
        this.Nationality = celebrity.Nationality;
        this.ReqPhotoPath = celebrity.ReqPhotoPath;
        return true;
    }
}

public class Lifeevent {
    public Lifeevent() { this.Description = string.Empty; }
    public int Id { get; set; }
    public int CelebrityId { get; set; }
    public DateTime Date { get; set; }
    public string Description { get; set; }
    public string? ReqPhotoPath { get; set; }
    public virtual bool Update(Lifeevent lifeevent) {
        this.CelebrityId = lifeevent.CelebrityId;
        this.Date = lifeevent.Date;
        this.Description = lifeevent.Description;
        return true;
    }
}

public interface IMix<T1, T2> {
    List<T2> GetLifeeventsByCelebrityId(int celebrityId);
    T1? GetCelebrityByLifeeventId(int lifeeventId);
}

public interface ICelebrity<T> : IDisposable {
    List<T> GetAllCelebrities();
    T? GetCelebrityById(int id);
    bool DelCelebrity(int id);
    bool AddCelebrity(T celebrity);
    bool UpdCelebrity(int id, T celebrity);
    int GetCelebrityIdByName(string name);
}

public interface ILifeevent<T> : IDisposable {
    List<T> GetAllLifeevents();
    T? GetLifeeventById(int id);
    bool DelLifeevent(int id);
    bool AddLifeevent(T lifeevent);
    bool UpdLifeevent(int id, T lifeevent);
}

public interface IRepository : IMix<Celebrity, Lifeevent>, ICelebrity<Celebrity>, ILifeevent<Lifeevent> { }