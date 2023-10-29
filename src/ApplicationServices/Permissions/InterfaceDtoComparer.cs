using Core.DTOs.Permissions;

namespace ApplicationServices.Permissions
{
    internal class InterfaceDtoComparer : IEqualityComparer<InterfaceDto>
    {
        public bool Equals(InterfaceDto? x, InterfaceDto? y)
        {
            return x!.Name == y!.Name;
        }

        public int GetHashCode(InterfaceDto obj)
        {
            return obj.Name.GetHashCode();
        }
    }
}
