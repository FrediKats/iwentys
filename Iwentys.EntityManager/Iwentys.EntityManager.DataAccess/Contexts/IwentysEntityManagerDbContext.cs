using Iwentys.EntityManager.Domain;
using Iwentys.EntityManager.Domain.Accounts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Iwentys.EntityManager.DataAccess;

public class IwentysEntityManagerDbContext : DbContext, IAccountManagementDbContext, IStudyDbContext
{
    private readonly IDbContextSeeder _dbContextSeeder;

    public DbSet<IwentysUser> IwentysUsers { get; set; }
    public DbSet<UniversitySystemUser> UniversitySystemUsers { get; set; }

    public DbSet<Student> Students { get; set; }
    public DbSet<StudyGroup> StudyGroups { get; set; }
    public DbSet<StudyProgram> StudyPrograms { get; set; }
    public DbSet<Subject> Subjects { get; set; }
    public DbSet<GroupSubject> GroupSubjects { get; set; }
    public DbSet<GroupSubjectMentor> GroupSubjectMentors { get; set; }
    public DbSet<StudyCourse> StudyCourses { get; set; }

    public IwentysEntityManagerDbContext(DbContextOptions<IwentysEntityManagerDbContext> options, IDbContextSeeder dbContextSeeder) : base(options)
    {
        _dbContextSeeder = dbContextSeeder;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.OnStudyModelCreating();

        _dbContextSeeder.Seed(modelBuilder);

        RemoveCascadeDeleting(modelBuilder);
        base.OnModelCreating(modelBuilder);
    }

    //TODO: temp hack
    private static void RemoveCascadeDeleting(ModelBuilder modelBuilder)
    {
        IEnumerable<IMutableForeignKey> cascadeFKs = modelBuilder.Model.GetEntityTypes()
            .SelectMany(t => t.GetForeignKeys())
            .Where(fk => !fk.IsOwnership && fk.DeleteBehavior == DeleteBehavior.Cascade);

        foreach (IMutableForeignKey fk in cascadeFKs)
            fk.DeleteBehavior = DeleteBehavior.Restrict;
    }
}