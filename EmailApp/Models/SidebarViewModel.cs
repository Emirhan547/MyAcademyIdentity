using EmailApp.Entities;

namespace EmailApp.Models
{
    public class SidebarViewModel
    {
        public int InboxCount { get; set; }
        public int SentCount { get; set; }
        public int DraftCount { get; set; }
        public int TrashCount { get; set; }
        public Dictionary<string, int> CategoryCounts { get; set; } = new();
        public Dictionary<string, string> CategoryColors { get; set; } = new();
        public Dictionary<string, string> CategoryTextColors { get; set; } = new();
    }
}
