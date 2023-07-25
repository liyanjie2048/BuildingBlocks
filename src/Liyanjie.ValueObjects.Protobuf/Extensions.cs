namespace ProtoBuf.Meta;

public static class Extensions
{
    public static void AddValueObjects(this RuntimeTypeModel model)
    {
        model.Add(typeof(Address), false).Add(nameof(Address.ADCode), nameof(Address.ADDisplay), nameof(Address.Detail));
        model.Add(typeof(Consignee), false).Add(nameof(Consignee.Address), nameof(Consignee.Contact));
        model.Add(typeof(Contact), false).Add(nameof(Contact.Type), nameof(Contact.Name), nameof(Contact.Number));
        model.Add(typeof(Geolocation), false).Add(nameof(Geolocation.Longitude), nameof(Geolocation.Latitude));
        model.Add(typeof(Identity), false).Add(nameof(Identity.Type), nameof(Identity.Value));
        model.Add(typeof(Job), false).Add(nameof(Job.Industry), nameof(Job.Company), nameof(Job.Position), nameof(Job.Address));
        model.Add(typeof(Licence), false).Add(nameof(Licence.Type), nameof(Licence.Number), nameof(Licence.Name), nameof(Licence.Pictures), nameof(Licence.IsIdentified));
        model.Add(typeof(Name), false).Add(nameof(Name.GivenName), nameof(Name.MiddleName), nameof(Name.FamilyName));
        model.Add(typeof(Operator), false).Add(nameof(Operator.Status), nameof(Operator.Identity));
        model.Add(typeof(School), false).Add(nameof(School.Type), nameof(School.Name), nameof(School.AdmissionDate), nameof(School.GraduatedDate));
        model.Add(typeof(Score), false).Add(nameof(Score.Total), nameof(Score.Count));
        model.Add(typeof(Shipment), false).Add(nameof(Shipment.Identity), nameof(Shipment.TrackingNumber));
        model.Add(typeof(Status), false).Add(nameof(Status.Value), nameof(Status.ChangeTime), nameof(Status.Remark));
    }
    public static void AddValueObject_Contact<TType, TName>(this RuntimeTypeModel model)
    {
        model.Add(typeof(Contact<TType, TName>), false).Add(nameof(Contact.Type), nameof(Contact.Name), nameof(Contact.Number));
    }
    public static void AddValueObject_Identity<TType, TValue>(this RuntimeTypeModel model)
    {
        model.Add(typeof(Identity<TType, TValue>), false).Add(nameof(Identity.Type), nameof(Identity.Value));
    }
    public static void AddValueObject_Job<TIndustry>(this RuntimeTypeModel model)
    {
        model.Add(typeof(Job<TIndustry>), false).Add(nameof(Job.Industry), nameof(Job.Company), nameof(Job.Position), nameof(Job.Address));
    }
    public static void AddValueObject_Licence<TType>(this RuntimeTypeModel model)
    {
        model.Add(typeof(Licence<TType>), false).Add(nameof(Licence.Type), nameof(Licence.Number), nameof(Licence.Name), nameof(Licence.Pictures), nameof(Licence.IsIdentified));
    }
    public static void AddValueObject_Operator<TStatus, TIdentity>(this RuntimeTypeModel model)
    {
        model.Add(typeof(Operator<TStatus, TIdentity>), false).Add(nameof(Operator.Status), nameof(Operator.Identity));
    }
    public static void AddValueObject_Range<TValue>(this RuntimeTypeModel model) where TValue : struct, IEquatable<TValue>
    {
        model.Add(typeof(Range<TValue>), false).Add(nameof(Range<TValue>.From), nameof(Range<TValue>.To));
    }
    public static void AddValueObject_School<TType>(this RuntimeTypeModel model)
    {
        model.Add(typeof(School<TType>), false).Add(nameof(School.Type), nameof(School.Name), nameof(School.AdmissionDate), nameof(School.GraduatedDate));
    }
    public static void AddValueObject_Shipment<TIdentity>(this RuntimeTypeModel model)
    {
        model.Add(typeof(Shipment<TIdentity>), false).Add(nameof(Shipment.Identity), nameof(Shipment.TrackingNumber));
    }
    public static void AddValueObject_Status<TValue>(this RuntimeTypeModel model)
    {
        model.Add(typeof(Status<TValue>), false).Add(nameof(Status.Value), nameof(Status.ChangeTime), nameof(Status.Remark));
    }
}
