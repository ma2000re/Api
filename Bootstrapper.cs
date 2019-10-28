namespace Api
{
    using Common.Models;
    using FluentNHibernate.Cfg;
    using FluentNHibernate.Cfg.Db;
    using Nancy;
    using Nancy.TinyIoc;
    using NHibernate;
    using NHibernate.Tool.hbm2ddl;
    using System;
    using Common;
    using Nancy.Bootstrapper;
    using Nancy.Authentication.Basic;

    public class Bootstrapper : DefaultNancyBootstrapper
    {
        private readonly ISessionFactory _sessionFactory;

        public Bootstrapper()
        {
            _sessionFactory = CreateSessionFactory();

            Book b = new Book();
            b.Author = "Hansi";
            b.InventoryNumber = "B1";
            b.Title = "Servus";
            b.Lended = false;

            User u = new User();
            u.Email = "test@gmx.at";
            u.Username = "test";

            Lending l = new Lending();
            l.DateOfLend = DateTime.Now;
            l.User = u;
            l.Book = b;

            Firmendaten fd = new Firmendaten();
            fd.Name = "BioBauernhof Lahner";
            fd.Rechtsform = "";
            fd.Sitz = "";
            fd.Firmenbuchnummer = "";
            fd.Anschrift = "";
            fd.Email = "";
            fd.Telefon = "+43 650 4381484";
            fd.MitgliedWKO = "";
            fd.Aufsichtsbehörde = "";
            fd.Berufsbezeichnung = "";
            fd.UIDNummer = "";
            fd.Datum = DateTime.Now;

            Art art = new Art();
            art.Bezeichnung = "Kontakt";

            Login lo = new Login();
            lo.Benutzername = "Manuel.Reisinger";
            lo.Passwort = "Passw0rd";
            lo.LetzteAnmeldung = Convert.ToDateTime("21.10.2019");

            Termine te = new Termine();
            te.Titel = "Ab Hof Verkauf";
            te.Beschreibung = "Alle Produkte im Ab Hof Verkauf!";
            te.DatumVon = new DateTime(2019,12,12);
            te.Login = lo;

            Postleitzahl pl = new Postleitzahl();
            pl.PLZ = "2124";
            pl.Ortschaft = "Oberkreuzstetten";

            Kunden ku = new Kunden();
            ku.Anrede = "Herr";
            ku.Vorname = "Max";
            ku.Nachname = "Mustermann";
            ku.Telefonnummer = "06602180625";
            ku.Email = "maxi@mustermann.com";
            ku.Strasse = "Gartenweg 1";
            ku.Postleitzahl = pl;
            ku.Aktiv = true;

            Bestellung be = new Bestellung();
            be.Kunde = ku;
            be.Status = "neu";
            be.Lieferdatum = new DateTime(2019,10,26);

            Formular fo = new Formular();
            fo.Vorname = "Max";
            fo.Nachname = "Mustermann";
            fo.Telefonnummer = "54654646";
            fo.Email = "max@gmail.com";
            fo.Inhalt = "10 Eier";
            fo.Datum = DateTime.Now;
            fo.Art = art;
            fo.Bestellung = be;

            Lieferanten li = new Lieferanten();
            li.Vorname = "Doris";
            li.Nachname = "Lahner";
            li.Firma = "BioBauernhof Lahner";
            li.Telefonnummer = "13546489";
            li.Email = "biobauernhof@lahner.at";
            li.Strasse = "Hauptstraße 161";
            li.Postleitzahl = pl;
            li.Aktiv = true;

            Artikel ar = new Artikel();
            ar.ExterneID = 5555;
            ar.Bezeichnung = "Hokkaido Kürbis";
            ar.PreisNetto = 1.20;
            ar.Ust = 20;
            ar.Lieferant = li;
            ar.Lagerstand = 10;
            ar.Reserviert = 2;
            ar.Aktiv = true;

            Rechnung re = new Rechnung();
            re.Datum = new DateTime(2019, 10, 26);
            re.Bezahlt = false;
            re.Kunde = ku;
            re.Bestellung = be;

            RechnungArtikel reAr = new RechnungArtikel();
            reAr.Rechnung = re;
            reAr.Artikel = ar;
            reAr.Menge = 2;
            reAr.NettoPreis = 1.20;
            reAr.Ust = 20;

            BestellungArtikel beAr = new BestellungArtikel();
            beAr.Bestellung = be;
            beAr.Artikel = ar;
            beAr.Menge = 2;
            beAr.Nettopreis = 1.20;
            beAr.Ust = 20;
            


            var session = _sessionFactory.OpenSession();

            using (var tran = session.BeginTransaction())
            {
                try
                {
                    //session.Save(b);
                    //session.Save(u);
                    //session.Save(l);
                    //session.Save(fd);
                    //session.Save(art);
                    //session.Save(fo);
                    //session.Save(lo);
                    //session.Save(te);
                    //session.Save(pl);
                    //session.Save(ku);
                    //session.Save(be);
                    //session.Save(li);
                    //session.Save(ar);
                    //session.Save(re);
                    //session.Save(reAr);
                    //session.Save(beAr);
                    tran.Commit();
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    throw ex;
                }
            }


        }

        protected override void ApplicationStartup(TinyIoCContainer container, Nancy.Bootstrapper.IPipelines pipelines)
        {
            base.ApplicationStartup(container, pipelines);

            pipelines.EnableBasicAuthentication(new BasicAuthenticationConfiguration(container.Resolve<IUserValidator>(),"Projektname"));
        }

        protected override void ConfigureRequestContainer(TinyIoCContainer container, NancyContext context)
        {
            base.ConfigureRequestContainer(container, context);
            container.Register((c, o) => _sessionFactory.OpenSession());
        }

        public static ISessionFactory CreateSessionFactory()
        {
            return Fluently
                .Configure()
                //.Database(MySQLConfiguration.Standard.ConnectionString("Server=[ServerIp]; Port=3306;Database=[Database]; Uid=[Username]; Pwd=[Password];"))
                .Database(MySQLConfiguration.Standard.ConnectionString("Server=127.0.0.1; Port=3306;Database=FutureFarm; Uid=root;"))
                .Mappings(m => m.FluentMappings.AddFromAssemblyOf<Book>())
                //uncomment to update schema db 
                .ExposeConfiguration(cfg => new SchemaUpdate(cfg).Execute(false, true)) 
                //uncoment to create schema db, each time the app is launched the db will be created
                //.ExposeConfiguration(cfg => new SchemaExport(cfg).Create(true, true))
              .BuildSessionFactory();
        }
    }
}