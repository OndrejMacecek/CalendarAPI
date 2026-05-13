# &#x20;

# CalendarAPI

Technický úkol — Kalendářové API

## Architektura aplikace
Aplikaci jsem rozdělil do několika vrstev:
* **Application**
  Vrstva zodpovědná za aplikační logiku. Validuje vstupní data z REST API, řeší aplikační chyby a určuje pořadí jednotlivých operací. komunikuje s domenou a s persistenci
* **Domain**
  Vrstva obsahující business logiku. Jedná se o jádro celé aplikace, které definuje chování jednotlivých entit a agregátů v systému.
* **Persistence**
  Vrstva zajišťující komunikaci s vnějším světem, například s databází, REST API, reportingem nebo messagingem.

Toto rozvrstvení umožňuje paralelní vývoj jednotlivých částí aplikace. Důležité je pouze předem domluvit datové kontrakty mezi vrstvami.

## Použité technologie a přístup
Pro jednoduchost jsem jako úložiště zvolil **SQLite**, které je součástí projektu `CalendarApi`. Jako datovou vstvu jsem požil **Entity Framework Core + migrations.**
Pro komunikaci mezi aplikační vrstvou a REST API jsem použil **MediatR** spolu s validací pomocí **FluentValidation**.
MediatR jsem rozdělil na:
**Commands**
**Queries**
Tím je v aplikaci použitý vzor **CQRS**.

Commands pracují nad živými daty přes doménovou vrstvu, kde je nutné mapování na doménové objekty. Queries naopak přistupují přímo do persistence vrstvy, kde se plní DTO objekty určené pouze pro návrat dat.

## Rozsah implementace
Při čtení zadání mi bylo jasné, že při plnohodnotném a poctivém zpracování by implementace zabrala několik dní. Proto jsem se rozhodl některé funkcionality zjednodušit nebo omezit.
Vytvořil jsem pouze pět základních tabulek:

* Uživatel
* Kalendář
* Typ události
* Událost
* Pozvánka

### Uživatel
Jednoduchá entita obsahující jméno a e-mail.

### Kalendář
Jednoduchá entita obsahující název, referenci na uživatele a časovou lokalizaci.

### Typ události
Určuje, jaké typy událostí mohou být v kalendáři vytvořeny pro uživatele, kalendář nebo systémové události.

### Kalendářní událost
Kalendářní události aktuálně používají pro určení časového intervalu hodnoty typu `DateTimeOffset`.
V robustnějším řešení by bylo vhodnější ukládat samostatně rok, měsíc, den a týden jako celočíselné hodnoty a tyto sloupce indexovat v databázi kvůli rychlejšímu vyhledávání.

### Kalendářní pozvánky
Pozvánka existuje vždy pouze jako jedna entita, i když v sobě obsahuje více pozvaných uživatelů.
Pozvánky jsou evidované pouze v rámci entity pozvánky. Pokud uživatel pozvánku přijme, změní se pouze její status kvůli viditelnosti. Žádný další proces se nespouští.
Toto řešení beru jako výrazně zjednodušené a z pohledu výkonu ne ideální. V plnohodnotné implementaci by bylo vhodnější vytvořit pozvanému uživateli vlastní událost s referencí na původní event. To by ale zároveň přineslo další režii při mazání, aktualizacích a řešení časových kolizí.

## Poznámka k SQLite a časovým pásmům
SQLite mě trochu překvapilo tím, že neumí nativně pracovat s `DateTimeOffset`. Proto jsem použil konverzi na `DateTime`.
Nejsem si ale úplně jistý, zda toto řešení bude správně fungovat při requestech z různých časových pásem. V produkčním řešení by bylo vhodné tuto část důkladněji otestovat a případně ukládat časy jednotně v UTC spolu s informací o časové zóně.

## Opakující se události
Podpora opakujících se událostí není implementována záměrně.
Jejich správná implementace výrazně zvyšuje složitost systému — například kvůli úpravám jednotlivých výskytů, mazání části série, časovým pásmům nebo synchronizaci pozvánek a nedostatečnému datovému modelu.
Vzhledem k rozsahu úkolu jsem se proto soustředil hlavně na architekturu aplikace a základní doménový model.

## Poznámky k návrhu
Některé části jsou záměrně zjednodušené kvůli časové náročnosti úkolu. Cílem bylo hlavně ukázat návrh architektury, práci s doménovou logikou a oddělení zodpovědností mezi vrstvami aplikace.

## Co bych řešil v produkční verzi
- propracovanější pozvánky
- opakující se události
- lepší práci s časovými pásmy
- optimalizaci databázových indexů
- oddělení read modelu
- caching
- audit log
- autentizaci a autorizaci
- souběhy requestu
- messaging
