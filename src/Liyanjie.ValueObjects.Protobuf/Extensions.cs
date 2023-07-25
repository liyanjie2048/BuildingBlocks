namespace ProtoBuf.Meta;

public static class Extensions
{
    public static void AddValueObjects(this RuntimeTypeModel model)
    {
        model.Add(typeof(Address), false).Add(nameof(Address.ADCode), nameof(Address.ADDisplay), nameof(Address.Detail));
        model.Add(typeof(Consignee), false).Add(nameof(Consignee.Address), nameof(Consignee.Contact));
        model.Add(typeof(Contact<,>), false).Add(nameof(Contact.Type), nameof(Contact.Name), nameof(Contact.Number));
        model.Add(typeof(Contact), false).Add(nameof(Contact.Type), nameof(Contact.Name), nameof(Contact.Number));
        model.Add(typeof(Geolocation), false).Add(nameof(Geolocation.Longitude), nameof(Geolocation.Latitude));
        model.Add(typeof(Identity<,>), false).Add(nameof(Identity.Type), nameof(Identity.Value));
        model.Add(typeof(Identity), false).Add(nameof(Identity.Type), nameof(Identity.Value));
        model.Add(typeof(Job<>), false).Add(nameof(Job.Industry), nameof(Job.Company), nameof(Job.Position), nameof(Job.Address));
        model.Add(typeof(Job), false).Add(nameof(Job.Industry), nameof(Job.Company), nameof(Job.Position), nameof(Job.Address));
        model.Add(typeof(Licence<>), false).Add(nameof(Licence.Type), nameof(Licence.Number), nameof(Licence.Name), nameof(Licence.Pictures), nameof(Licence.IsIdentified));
        model.Add(typeof(Licence), false).Add(nameof(Licence.Type), nameof(Licence.Number), nameof(Licence.Name), nameof(Licence.Pictures), nameof(Licence.IsIdentified));
        model.Add(typeof(Name), false).Add(nameof(Name.GivenName), nameof(Name.MiddleName), nameof(Name.FamilyName));
        model.Add(typeof(Operator<,>), false).Add(nameof(Operator.Status), nameof(Operator.Identity));
        model.Add(typeof(Operator), false).Add(nameof(Operator.Status), nameof(Operator.Identity));
        model.Add(typeof(Range<>), false).Add(nameof(Range<int>.From), nameof(Range<int>.To));
        model.Add(typeof(School<>), false).Add(nameof(School.Type), nameof(School.Name), nameof(School.AdmissionDate), nameof(School.GraduatedDate));
        model.Add(typeof(School), false).Add(nameof(School.Type), nameof(School.Name), nameof(School.AdmissionDate), nameof(School.GraduatedDate));
        model.Add(typeof(Score), false).Add(nameof(Score.Total), nameof(Score.Count));
        model.Add(typeof(Shipment<>), false).Add(nameof(Shipment.Identity), nameof(Shipment.TrackingNumber));
        model.Add(typeof(Shipment), false).Add(nameof(Shipment.Identity), nameof(Shipment.TrackingNumber));
        model.Add(typeof(Status<>), false).Add(nameof(Status.Value), nameof(Status.ChangeTime), nameof(Status.Remark));
        model.Add(typeof(Status), false).Add(nameof(Status.Value), nameof(Status.ChangeTime), nameof(Status.Remark));
    }
}
