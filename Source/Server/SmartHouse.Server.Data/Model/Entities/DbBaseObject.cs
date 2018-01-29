using System;
using DevExpress.Xpo;
using DevExpress.Xpo.Metadata;

namespace SmartHouse.Server.Data.Model.Entities {
	/// <summary>
	/// Базовый объект с сохранением информации о создании, изменении и удалении объекта
	/// </summary>
	public abstract class DbBaseObject : DbObject {
		protected DbBaseObject() { }
		protected DbBaseObject(Session session) : base(session) { }
		protected DbBaseObject(Session session, Guid gid) : base(session, gid) { }
		protected DbBaseObject(Session session, XPClassInfo classInfo) : base(session, classInfo) { }

		DateTime _fCreateTime;
		/// <summary> Дата и время создания объекта </summary>
		public DateTime CreateTime {
			get => _fCreateTime;
			set => SetPropertyValue<DateTime>(nameof(CreateTime), ref _fCreateTime, value);
		}

		DateTime _fModifyTime;
		/// <summary> Дата и время последнего изменения объекта </summary>
		public DateTime ModifyTime {
			get => _fModifyTime;
			set => SetPropertyValue<DateTime>(nameof(ModifyTime), ref _fModifyTime, value);
		}

		DateTime _fDeleteTime;

		/// <summary> Дата и время удаления объекта </summary>
		public DateTime DeleteTime {
			get => _fDeleteTime;
			set => SetPropertyValue<DateTime>(nameof(DeleteTime), ref _fDeleteTime, value);
		}

		protected override void OnSaving() {
			if (IsNew)
				CreateTime = DateTime.Now;
			else if (IsDirty)
				ModifyTime = DateTime.Now;
			base.OnSaving();
		}

		protected override void OnDeleting() {
			DeleteTime = DateTime.Now;
			base.OnDeleting();
		}
	}
}
