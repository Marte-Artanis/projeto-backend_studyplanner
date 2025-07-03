using StudyPlanner;
using System.Globalization;
using StudyPlanner.Services;
using Microsoft.EntityFrameworkCore;

var db = new PlannerContext();
db.Database.Migrate();

var subjectSvc = new SubjectService(db);
var sessionSvc = new SessionService(db);
var statsSvc   = new StatisticsService(db);
var goalSvc    = new GoalService(db);

void PrintSubjects()
{
    var subjects = subjectSvc.GetAll().ToList();
    if (subjects.Count == 0)
    {
        Console.WriteLine("Nenhuma matéria cadastrada.");
        return;
    }
    foreach (var s in subjects)
        Console.WriteLine($"{s.Id}: {s.Name}");
}

while (true)
{
    Console.WriteLine("\n== StudyPlanner ==");
    Console.WriteLine("1) Adicionar matéria");
    Console.WriteLine("2) Listar matérias");
    Console.WriteLine("3) Registrar sessão");
    Console.WriteLine("4) Listar sessões");
    Console.WriteLine("5) Estatísticas");
    Console.WriteLine("6) Listar metas");
    Console.WriteLine("7) Adicionar meta");
    Console.WriteLine("0) Sair");
    Console.Write("Escolha: ");
    var choice = Console.ReadLine();

    switch (choice)
    {
        case "1":
            Console.Write("Nome da matéria: ");
            var name = Console.ReadLine() ?? string.Empty;
            try
            {
                subjectSvc.Add(name);
                Console.WriteLine("Matéria salva!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro: {ex.Message}");
            }
            break;

        case "2":
            PrintSubjects();
            break;

        case "3":
            if (!subjectSvc.GetAll().Any())
            {
                Console.WriteLine("Cadastre uma matéria antes de registrar sessões.");
                break;
            }
            PrintSubjects();
            Console.Write("Id da matéria: ");
            if (!int.TryParse(Console.ReadLine(), out var subjId))
            {
                Console.WriteLine("Id inválido.");
                break;
            }
            Console.Write("Início (yyyy-MM-dd HH:mm): ");
            if (!DateTime.TryParseExact(Console.ReadLine(), "yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out var start))
            {
                Console.WriteLine("Data/hora inválida.");
                break;
            }
            Console.Write("Fim (yyyy-MM-dd HH:mm): ");
            if (!DateTime.TryParseExact(Console.ReadLine(), "yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out var end) || end <= start)
            {
                Console.WriteLine("Data/hora inválida ou anterior ao início.");
                break;
            }
            Console.Write("Notas (opcional): ");
            var notes = Console.ReadLine();
            try
            {
                sessionSvc.AddSession(subjId, start, end, notes);
                Console.WriteLine("Sessão registrada!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro: {ex.Message}");
            }
            break;

        case "4":
            var sess = sessionSvc.GetSessionsWithSubject()
                                   .OrderBy(s => s.Start)
                                   .Select(s => new
                                   {
                                       s.Id,
                                       SubjectName = s.Subject.Name,
                                       s.Start,
                                       s.End,
                                       Hours = Math.Round((s.End - s.Start).TotalHours, 2)
                                   });
            if (!sess.Any())
            {
                Console.WriteLine("Nenhuma sessão registrada.");
                break;
            }
            foreach (var s in sess)
                Console.WriteLine($"{s.Id} | {s.SubjectName} | {s.Start:g} – {s.End:g} | {s.Hours} h");
            break;

        case "5":
            Console.WriteLine("Período: 1) 7 dias  2) 30 dias  3) Todo");
            var perChoice = Console.ReadLine();
            DateTime? from = perChoice switch
            {
                "1" => DateTime.Today.AddDays(-6),
                "2" => DateTime.Today.AddDays(-29),
                _ => null
            };

            var totals = statsSvc.GetTotalsBySubject(from);
            if (!totals.Any())
            {
                Console.WriteLine("Nenhuma sessão no período selecionado.");
            }
            else
            {
                Console.WriteLine("\nHoras por matéria:");
                foreach (var t in totals)
                    Console.WriteLine($"{t.Subject}: {t.Hours} h");
            }
            Console.WriteLine($"\nStreak atual: {statsSvc.GetCurrentStreak()} dia(s).");
            break;

        case "6":
            var gpList = goalSvc.GetGoalProgress().ToList();
            if (!gpList.Any())
            {
                Console.WriteLine("Nenhuma meta cadastrada.");
                break;
            }
            Console.WriteLine("\nMetas:");
            foreach (var gp in gpList)
            {
                Console.WriteLine($"{gp.Goal.Id}: {gp.Goal.Subject.Name} -> {gp.HoursDone}/{gp.Goal.TargetHours} h ({gp.ProgressPct}% ) Status: {gp.Status}");
            }
            break;

        case "7":
            if (!subjectSvc.GetAll().Any())
            {
                Console.WriteLine("Cadastre uma matéria antes de criar metas.");
                break;
            }
            PrintSubjects();
            Console.Write("Id da matéria: ");
            if (!int.TryParse(Console.ReadLine(), out var subGoalId))
            {
                Console.WriteLine("Id inválido.");
                break;
            }
            Console.Write("Horas alvo: ");
            if (!double.TryParse(Console.ReadLine(), out var targetHours) || targetHours <= 0)
            {
                Console.WriteLine("Valor inválido.");
                break;
            }
            Console.Write("Deadline (yyyy-MM-dd): ");
            if (!DateTime.TryParseExact(Console.ReadLine(), "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out var deadline))
            {
                Console.WriteLine("Data inválida.");
                break;
            }
            try
            {
                goalSvc.AddGoal(subGoalId, targetHours, deadline);
                Console.WriteLine("Meta adicionada!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro: {ex.Message}");
            }
            break;

        case "0":
            return;

        default:
            Console.WriteLine("Opção inválida.");
            break;
    }
}
