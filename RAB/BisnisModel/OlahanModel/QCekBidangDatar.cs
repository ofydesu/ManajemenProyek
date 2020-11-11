using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Identity;
using RAB.Asset.Enum;
using RAB.Asset.Obyek;
using RAB.Data;
using RAB.Models.Utama;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace RAB.Asset.OlahanModel
{
    #region class partial untuk bantuan
    public class AVektorCek
	{
		public Koordinat Koord { get; set; }
		public bool Dipakai { get; set; }
	}
	interface IVektorSikuKiri
	{
		public List<Koordinat> LstKoord { get; set; }
		public AVektorCek KiriBawah { get; set; }
		public AVektorCek KiriAtas { get; set; }
		public AVektorCek KananBawah { get; set; }
	}
	public class AVektorSikuKiri //: IVektorSikuKiri
    {
		public List<Koordinat> LstKoord { get; set; }
		public int MaxY { get; set; }
		public AVektorCek KiriBawah { get; set; }
		public AVektorCek KiriAtas { get; set; }
		public AVektorCek KananBawah { get; set; }
	}
	public class AVektorSikuAll : AVektorSikuKiri
	{
		public AVektorCek KananAtas { get; set; }
	}
	public class AVektorCekAtas : AVektorSikuAll
	{
		public AVektorCek AkhirAtas { get; set; }
	}
	public class ACekGarisTerpakai
	{
		public OGaris Garis { get; set; }
		public bool Dipakai { get; set; }
	}
    public class AGarisTerpakai
	{
		public ACekGarisTerpakai DatarBawah { get; set; }
		public ACekGarisTerpakai DatarAtas { get; set; }
		public ACekGarisTerpakai TegakKiri { get; set; }
		public ACekGarisTerpakai TegakKanan { get; set; }
	}
	#endregion
	public class QCekBidangDatar
    {
		private readonly RabContext _context;
		private int _polaId;
		public QCekBidangDatar(RabContext context, int polaId)
		{
			_context = context;
			_polaId = polaId;
		}
        
		private Koordinat CekKoordKananBawah(List<Koordinat> lstKoord, int Xid, int Yid, bool tX, bool tY, int maxX)
        {
			if (!tX && !tY)
            {
				try{
					var lstDatar = lstKoord.Where(p2 => p2.TitikY.SumbuId == Yid)
										.Where(p3 => p3.TitikX.SumbuId > Xid)
										.Where(p4 => p4.SbgAkhirX || p4.TidakKeY == false || p4.TitikX.SumbuId == maxX);
					if (lstDatar != null){
						return lstDatar.First();
					}
                }
                catch{}

			}
			return new Koordinat();
        }
		private Koordinat CekKoordKiriAtas(List<Koordinat> lstKoord, int Xid, int Yid, bool tX, bool tY, int maxY)
		{
			if (!tX && !tY)
			{
				try{
					var lstDatar = lstKoord.Where(p2 => p2.TitikY.SumbuId > Yid)
										.Where(p3 => p3.TitikX.SumbuId == Xid)
										.Where(p4 => p4.SbgAkhirY || p4.TidakKeX == false || p4.TitikY.SumbuId == maxY);
					if (lstDatar != null){
						return lstDatar.First();
					}
				}
				catch {}
			}

			return new Koordinat();
		}
		public IQueryable<AVektorSikuKiri> QrySikuKiri
        {
			get
			{
                QCekKoordinat qKoord = new QCekKoordinat(_context, _polaId);
                var qKoorTblIni = qKoord.QryTblIniPosUpToDate.ToList();

				var maxSb = qKoord.CekMaxSumbuId(qKoorTblIni);

				var sikuKiri = qKoorTblIni
						.Select(p => new AVektorSikuKiri
						{
							KiriBawah = new AVektorCek
							{
								Koord = qKoorTblIni.Where(k=>k.KoordId == p.KoordId).First(),
								Dipakai = true
							},
							KananBawah = new AVektorCek
							{
								Koord =	CekKoordKananBawah(qKoorTblIni, p.TitikX.SumbuId,p.TitikY.SumbuId,p.TidakKeX,p.TidakKeY, maxSb.X),
								Dipakai = !p.TidakKeY && !p.TidakKeX
							},

                            KiriAtas = new AVektorCek
                            {
                                Koord = CekKoordKiriAtas(qKoorTblIni,p.TitikX.SumbuId, p.TitikY.SumbuId, p.TidakKeX, p.TidakKeY, maxSb.Y),
                                Dipakai = !p.TidakKeX && !p.TidakKeY
							},
							LstKoord = qKoorTblIni,
							MaxY = maxSb.Y
                        });
				var fSikuKiri = sikuKiri.Where(p => p.KananBawah.Dipakai == true);
				return fSikuKiri.AsQueryable();
			}

		}
		private AVektorCek CekKoorKananAtas(List<Koordinat>lstKoord, Koordinat kananBawahKoord, Koordinat kiriAtasKoord, int maxY)
        {
			if (kananBawahKoord != null && kiriAtasKoord != null)
            {
                try
                {
					var koord = lstKoord.Where(p => p.TitikX == kananBawahKoord.TitikX && p.TitikY == kiriAtasKoord.TitikY).Single();
					if (koord != null) {
						return new AVektorCek(){
							Koord = koord,
							Dipakai = koord.TitikY.SumbuId == maxY
						};
					}
                }catch{}
            }
			return new AVektorCek();
		}
		public IQueryable<AVektorSikuAll> QrySikuAll
        {
            get
            {
                var lstSikuKiri = QrySikuKiri.ToList();
                var ttkKananAtas = QrySikuKiri.ToList()
					.Select(g => new AVektorSikuAll {
					KiriBawah = g.KiriBawah,
					KananBawah = g.KananBawah,
					KiriAtas = g.KiriAtas,
					KananAtas = CekKoorKananAtas(g.LstKoord, g.KananBawah.Koord,g.KiriAtas.Koord, g.MaxY),
					LstKoord = g.LstKoord.Where(p => p.TidakKeX == false && p.TidakKeY == false).ToList()
					});
				return ttkKananAtas.AsQueryable();
			}
		}
		public IQueryable<AGarisTerpakai> QryGarisBidang
		{
			get
			{
				var garisBdg = QrySikuAll.ToList().Select(t => new AGarisTerpakai
				{
					DatarBawah = new ACekGarisTerpakai
					{
						Garis = new OGaris
						{							
							Awal = t.KiriBawah.Koord,
							Akhir = t.KananBawah.Koord,
							Arah = ESumbu.X
						},
						Dipakai = true
					},

					TegakKiri = new ACekGarisTerpakai
					{
						Garis = new OGaris
						{
							Awal = t.KiriBawah.Koord,
							Akhir = t.KiriAtas.Koord,
							Arah = ESumbu.Y
						},
						Dipakai = true
					},

					TegakKanan = new ACekGarisTerpakai
					{
						Garis = new OGaris
						{
							Awal = t.KananBawah.Koord,
							Akhir = t.KananAtas.Koord,
							Arah = ESumbu.Y
						},
						Dipakai = !t.LstKoord.Exists(l => l.Nama== t.KananBawah.Koord.Nama)
					},

					DatarAtas = new ACekGarisTerpakai
					{
						Garis = new OGaris
						{
							Awal = t.KiriAtas.Koord,
							Akhir = t.KananAtas.Koord,
							Arah = ESumbu.X
						},
						Dipakai = t.KananAtas.Dipakai,
					}
				}); 
				
				return garisBdg.AsQueryable();
			}
		}
		public IQueryable<OGaris> QryGarisPartial
        {
            get
            {
				var dtrAtas = QryGarisBidang.Select(p => new ACekGarisTerpakai
				{
					Garis = p.DatarAtas.Garis,
					Dipakai = false //p.DatarAtas.Dipakai
				});
				var dtrBawah = QryGarisBidang.Select(p => new ACekGarisTerpakai
				{
					Garis = p.DatarBawah.Garis,
					Dipakai = p.DatarBawah.Dipakai
				});

				var tgkKiri = QryGarisBidang.Select(p => new ACekGarisTerpakai
				{
					Garis = p.TegakKiri.Garis,
					Dipakai = p.TegakKiri.Dipakai
				});

				var tgkKanan = QryGarisBidang.Select(p => new ACekGarisTerpakai
				{
					Garis = p.TegakKanan.Garis,
					Dipakai = false	// p.TegakKanan.Dipakai
				});

				var gabtegak = tgkKanan.Union(tgkKiri).Where(g=>g.Dipakai == true);
				var gabdatar = dtrAtas.Union(dtrBawah).Where(d => d.Dipakai == true);
				var oGab = gabtegak.GroupBy(g => g.Garis.Akhir.Nama).Select(s => s.First());
				oGab = oGab.Union(gabdatar).OrderBy(o => o.Garis.Nama);
				var grsBidang = oGab.Select(g => g.Garis);


				return grsBidang.Union(QryGarisTunggal).Union(QryGarisMiring).AsQueryable();
            }
        }

		private Koordinat CekDatarTggl(List<Koordinat> lstKoord, int Xid, int Yid, bool xAwal, int maxX)
		{
			if (xAwal)
			{
				try
				{
					var lstDatar = lstKoord.Where(p2 => p2.TitikY.SumbuId == Yid)
										.Where(p3 => p3.TitikX.SumbuId > Xid)
										.Where(p4 => p4.SbgAkhirX || p4.TidakKeY == false || p4.TitikX.SumbuId == maxX);
					if (lstDatar != null)
					{
						return lstDatar.First();
					}
				}
				catch { }

			}
			return new Koordinat();
		}
		private Koordinat CekTegakTggl(List<Koordinat> lstKoord, int Xid, int Yid, bool yAwal, int maxY)
		{
			if (yAwal)
			{
				try
				{
					var lstDatar = lstKoord.Where(p2 => p2.TitikY.SumbuId > Yid)
										.Where(p3 => p3.TitikX.SumbuId == Xid)
										.Where(p4 => p4.SbgAkhirY || p4.TidakKeX == false || p4.TitikY.SumbuId == maxY);
					if (lstDatar != null)
					{
						return lstDatar.First();
					}
				}
				catch { }

			}
			return new Koordinat();
		}
		public IQueryable<OGaris> QryGarisTunggal
		{
			get
			{
				QCekKoordinat qKoord = new QCekKoordinat(_context, _polaId);
				var qKoorTblIni = qKoord.QryTblIniPosUpToDate.ToList();
                var maxSb = qKoord.CekMaxSumbuId(qKoorTblIni);

				var qGrsTggl = qKoorTblIni
						.Select(p => new AGarisTerpakai(){
							DatarBawah = new ACekGarisTerpakai()
							{
								Garis = new OGaris()
								{
									Arah = ESumbu.X,
									Awal = p,
									Akhir = CekDatarTggl(qKoorTblIni, p.TitikX.SumbuId, p.TitikY.SumbuId, p.SbgAwalX, maxSb.X),
								},
								Dipakai = p.SbgAwalX
							},
							TegakKanan= new ACekGarisTerpakai()
							{
								Garis = new OGaris()
								{
									Arah = ESumbu.Y,
									Awal = p,
									Akhir = CekTegakTggl(qKoorTblIni, p.TitikX.SumbuId, p.TitikY.SumbuId, p.SbgAwalY, maxSb.Y),
								},
								Dipakai = p.SbgAwalY
							}
						});
				var grsDatar = qGrsTggl.Select(d => d.DatarBawah);
				var grsTegak= qGrsTggl.Select(d => d.TegakKanan);

				var grsTggl = grsDatar.Union(grsTegak).Where(d => d.Dipakai == true).Select(s=>s.Garis);
				return grsTggl.AsQueryable();
			}

		}
		private Koordinat CekMiringAtas(List<Koordinat> lstKoord, int Xid, int Yid, bool miringAtas, int maxY)
		{
			if (miringAtas)
			{
				try
				{
					var lstDatar = lstKoord.Where(p2 => p2.TitikY.SumbuId > Yid)
										.Where(p3 => p3.TitikX.SumbuId > Xid)
										.Where(p4 => p4.SbgAkhirY || p4.SbgAkhirX
												|| p4.SbgAwalY || p4.SbgAwalX
												|| !p4.TidakKeX || !p4.TidakKeY
												|| p4.TitikY.SumbuId == maxY);
					if (lstDatar != null)
					{
						return lstDatar.First();
					}
				}
				catch { }

			}
			return new Koordinat();
		}
		private Koordinat CekMiringBawah(List<Koordinat> lstKoord, int Xid, int Yid, bool miringBawah, int minY = 1)
		{
			if (miringBawah)
			{
				try
				{
					var lstDatar = lstKoord.Where(p2 => p2.TitikY.SumbuId < Yid)
										.Where(p3 => p3.TitikX.SumbuId > Xid)
										.Where(p4 =>
												p4.SbgAkhirY || 
												p4.SbgAkhirX ||
												p4.SbgAwalY || 
												p4.SbgAwalX ||
												!p4.TidakKeX || 
												!p4.TidakKeY ||
												p4.TitikY.SumbuId == minY
												).OrderByDescending(o=>o.TitikY.SumbuId);
					if (lstDatar != null)
					{
						return lstDatar.First();
					}
				}
				catch { }

			}
			return new Koordinat();
		}
		public IQueryable<OGaris> QryGarisMiring
		{
			get
			{
				QCekKoordinat qKoord = new QCekKoordinat(_context, _polaId);
				var qKoorTblIni = qKoord.QryTblIniPosUpToDate.ToList();
				var maxSb = qKoord.CekMaxSumbuId(qKoorTblIni);

				var qGrsTggl = qKoorTblIni
						.Select(p => new AGarisTerpakai()
						{
							// sebagai miring ke bawah
							DatarBawah = new ACekGarisTerpakai()
							{
								Garis = new OGaris()
								{
									Arah = ESumbu.Y,
									Awal = p,
									Akhir = CekMiringBawah(qKoorTblIni, p.TitikX.SumbuId, p.TitikY.SumbuId, p.MiringBawah,1),
								},
								Dipakai = p.MiringBawah
							},
							// sebagai miring Atas
							DatarAtas = new ACekGarisTerpakai()
							{
								Garis = new OGaris()
								{
									Arah = ESumbu.Y,
									Awal = p,
									Akhir = CekMiringAtas(qKoorTblIni, p.TitikX.SumbuId, p.TitikY.SumbuId, p.MiringAtas, maxSb.Y),
								},
								Dipakai = p.MiringAtas
							}
						});
				var grsMiringBawah = qGrsTggl.Select(d => d.DatarBawah);
				var grsMiringAtas = qGrsTggl.Select(d => d.DatarAtas);

				var grsTggl = grsMiringBawah.Union(grsMiringAtas).Where(d => d.Dipakai == true/* && d.Garis.Akhir == null*/).Select(s => s.Garis);
				return grsTggl.AsQueryable();
			}

		}
	}
}
