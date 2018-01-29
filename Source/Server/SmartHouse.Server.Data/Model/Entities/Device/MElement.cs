using System;
using DevExpress.Xpo;
using DevExpress.Xpo.Metadata;

namespace SmartHouse.Server.Data.Model.Entities.Device {
	public abstract class MElement : DbBaseObject {
		protected MElement() { }
		protected MElement(Session session) : base(session) { }
		protected MElement(Session session, Guid gid) : base(session, gid) { }
		protected MElement(Session session, XPClassInfo classInfo) : base(session, classInfo) { }

		string _fElenemtId;
		/// <summary> Идентификатор элемента </summary>
		public string ElementId {
			get => _fElenemtId;
			set => SetPropertyValue(nameof(ElementId), ref _fElenemtId, value);
		}



		EElementType _fType;
		/// <summary> Тип элемента </summary>
		public EElementType Type {
			get => _fType;
			set => SetPropertyValue(nameof(Type), ref _fType, value);
		}
	}
}
