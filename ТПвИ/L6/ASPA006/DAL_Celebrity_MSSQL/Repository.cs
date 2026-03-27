namespace DAL_Celebrity_MSSQL;
using DAL_Celebrity;

public class Repository : IRepository {
    Context context;
    public Repository() { this.context = new Context(); }
    public Repository(string connectionstring) { this.context = new Context(connectionstring); }
    
    public static IRepository Create() => new Repository();
    public static IRepository Create(string connectionstring) => new Repository(connectionstring);
    
    public List<Celebrity> GetAllCelebrities() => context.Celebrities.ToList();
    public Celebrity? GetCelebrityById(int id) => context.Celebrities.Find(id);
    public bool AddCelebrity(Celebrity c) { context.Celebrities.Add(c); return context.SaveChanges() > 0; }
    public bool DelCelebrity(int id) { var c = GetCelebrityById(id); if (c==null) return false; context.Celebrities.Remove(c); return context.SaveChanges() > 0; }
    public bool UpdCelebrity(int id, Celebrity c) { var t = GetCelebrityById(id); return t != null && t.Update(c) && context.SaveChanges() > 0; }
    public int GetCelebrityIdByName(string name) => context.Celebrities.FirstOrDefault(c => c.FullName.Contains(name))?.Id ?? 0;

    public List<Lifeevent> GetAllLifeevents() => context.Lifeevents.ToList();
    public List<Lifeevent> GetLifeeventsByCelebrityId(int cid) => context.Lifeevents.Where(e => e.CelebrityId == cid).ToList();
    
    public Lifeevent? GetLifeeventById(int id) => context.Lifeevents.Find(id);
    public bool AddLifeevent(Lifeevent l) { context.Lifeevents.Add(l); return context.SaveChanges() > 0; }
    public bool DelLifeevent(int id) { var l = GetLifeeventById(id); if (l==null) return false; context.Lifeevents.Remove(l); return context.SaveChanges() > 0; }
    public bool UpdLifeevent(int id, Lifeevent l) { var t = GetLifeeventById(id); return t != null && t.Update(l) && context.SaveChanges() > 0; }
    public Celebrity? GetCelebrityByLifeeventId(int leid) { var ev = GetLifeeventById(leid); return ev != null ? GetCelebrityById(ev.CelebrityId) : null; }

    public void Dispose() => context.Dispose();
}