using System;
using System.Linq;
using System.Linq.Expressions;
using DevExpress.Xpo;
using DevExpress.Xpo.Metadata;

namespace SmartHouse.Server.Data.Model.Entities {
	/// <summary>
	/// Базовый класс для все сущностей структуры данных
	/// </summary>
	[NonPersistent]
	public abstract class DbObject : XPCustomObject {

		/// <summary>
		/// Базовый конструктор для создания объекта
		/// </summary>
		protected DbObject() { }

		/// <summary>
		/// Базовый конструктор для создания объекта
		/// </summary>
		/// <param name="session">Сессия для доступа к базе</param>
		protected DbObject(Session session) : base(session) {}

		/// <summary>
		/// Базовый конструктор для создания объекта
		/// </summary>
		/// <param name="session">Сессия для доступа к базе</param>
		/// <param name="gid">Глобальный идентификатор объекта</param>
		protected DbObject(Session session, Guid gid) : base(session) {
			Gid = gid;
		}

		/// <summary>
		/// Базовый конструктор для создания объекта
		/// </summary>
		/// <param name="session">Сессия для доступа к базе</param>
		/// <param name="classInfo"></param>
		protected DbObject(Session session, XPClassInfo classInfo) : base(session, classInfo) {}

		long _fOid;
		/// <summary> Внутренний идентификатор объекта </summary>
		[Key(AutoGenerate = true)]
		public long Oid {
			get => _fOid;
			set => SetPropertyValue(nameof(Oid), ref _fOid, value);
		}

		Guid _fGid;
		/// <summary> Внешний идентификатор объекта </summary>
		[Indexed(Unique = true)]
		public Guid Gid {
			get => _fGid;
			set => SetPropertyValue(nameof(Gid), ref _fGid, value);
		}

		/// <summary> Новый объект </summary>
		[NonPersistent]
		public bool IsNew => Oid < 1;

		/// <summary> Измененный </summary>
		[NonPersistent]
		public bool IsDirty { get; private set; }

		protected override void OnChanged(string propertyName, object oldValue, object newValue) {
			if (propertyName != "CreateTime" && propertyName != "ModifyTime")
				IsDirty = true;
			base.OnChanged(propertyName, oldValue, newValue);
		}


		protected override void OnSaving() {
			if (Gid == Guid.Empty)
				Gid = Guid.NewGuid();
			base.OnSaving();
		}

		/// <summary>
		/// Запрос в бд
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="session"></param>
		/// <param name="predicate"></param>
		/// <returns></returns>
		public static IQueryable<T> Query<T>(Session session, Expression<Func<T, bool>> predicate) where T : DbObject {
			if(session == null)
				return null;
			return (predicate != null ? (session.Query<T>()).Where(predicate) : session.Query<T>()).AsQueryable();
		}
	}
}