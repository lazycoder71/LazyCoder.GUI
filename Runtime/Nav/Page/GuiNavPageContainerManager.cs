using System.Collections.Generic;
using LazyCoder.Core;

namespace LazyCoder.Gui
{
    public static class GuiNavPageContainerManager
    {
        // Key: groupIndex, Value: List of GuiNavPageContainers
        private static readonly Dictionary<int, List<GuiNavPageContainer>> Containers = new();

        public static GuiNavPageContainer GetContainer(int groupIndex)
        {
            // Check if the group index exists and has containers
            if (!Containers.ContainsKey(groupIndex) || Containers[groupIndex].IsNullOrEmpty())
                return null;

            // Always use the last container in the list
            return Containers[groupIndex].Last();
        }

        public static void AddContainer(GuiNavPageContainer container, int groupIndex)
        {
            // Ensure the group index exists in the dictionary
            if (!Containers.ContainsKey(groupIndex))
                Containers.Add(groupIndex, new List<GuiNavPageContainer>());

            // Add the container if it's not already present
            if (!Containers[groupIndex].Contains(container))
                Containers[groupIndex].Add(container);
        }

        public static void RemoveContainer(GuiNavPageContainer container, int groupIndex)
        {
            // Ensure the group index exists in the dictionary
            if (!Containers.ContainsKey(groupIndex))
                Containers.Add(groupIndex, new List<GuiNavPageContainer>());

            // Remove the container if it exists
            if (Containers[groupIndex].Contains(container))
                Containers[groupIndex].Remove(container);
        }
    }
}