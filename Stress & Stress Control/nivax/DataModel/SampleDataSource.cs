using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Windows.ApplicationModel.Resources.Core;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

// The data model defined by this file serves as a representative example of a strongly-typed
// model that supports notification when members are added, removed, or modified.  The property
// names chosen coincide with data bindings in the standard item templates.
//
// Applications may use this model as a starting point and build on it, or discard it entirely and
// replace it with something appropriate to their needs.

namespace PlanningDairyTemplate.Data
{
    /// <summary>
    /// Base class for <see cref="SampleDataItem"/> and <see cref="SampleDataGroup"/> that
    /// defines properties common to both.
    /// </summary>
    [Windows.Foundation.Metadata.WebHostHidden]
    public abstract class SampleDataCommon : PlanningDairyTemplate.Common.BindableBase
    {
        private static Uri _baseUri = new Uri("ms-appx:///");

        public SampleDataCommon(String uniqueId, String title, String subtitle, String imagePath, String description)
        {
            this._uniqueId = uniqueId;
            this._title = title;
            this._subtitle = subtitle;
            this._description = description;
            this._imagePath = imagePath;
        }

        private string _uniqueId = string.Empty;
        public string UniqueId
        {
            get { return this._uniqueId; }
            set { this.SetProperty(ref this._uniqueId, value); }
        }

        private string _title = string.Empty;
        public string Title
        {
            get { return this._title; }
            set { this.SetProperty(ref this._title, value); }
        }

        private string _subtitle = string.Empty;
        public string Subtitle
        {
            get { return this._subtitle; }
            set { this.SetProperty(ref this._subtitle, value); }
        }

        private string _description = string.Empty;
        public string Description
        {
            get { return this._description; }
            set { this.SetProperty(ref this._description, value); }
        }

        private ImageSource _image = null;
        private String _imagePath = null;
        public ImageSource Image
        {
            get
            {
                if (this._image == null && this._imagePath != null)
                {
                    this._image = new BitmapImage(new Uri(SampleDataCommon._baseUri, this._imagePath));
                }
                return this._image;
            }

            set
            {
                this._imagePath = null;
                this.SetProperty(ref this._image, value);
            }
        }

        public void SetImage(String path)
        {
            this._image = null;
            this._imagePath = path;
            this.OnPropertyChanged("Image");
        }
    }

    /// <summary>
    /// Generic item data model.
    /// </summary>
    public class SampleDataItem : SampleDataCommon
    {
        public SampleDataItem(String uniqueId, String title, String subtitle, String imagePath, String description, String content, SampleDataGroup group)
            : base(uniqueId, title, subtitle, imagePath, description)
        {
            this._content = content;
            this._group = group;
        }

        private string _content = string.Empty;
        public string Content
        {
            get { return this._content; }
            set { this.SetProperty(ref this._content, value); }
        }

        private SampleDataGroup _group;
        public SampleDataGroup Group
        {
            get { return this._group; }
            set { this.SetProperty(ref this._group, value); }
        }
        private string _createdon = string.Empty;
        public string CreatedOn
        {
            get { return this._createdon; }
            set { this.SetProperty(ref this._createdon, value); }
        }
        private string _createdtxt = string.Empty;
        public string CreatedTxt
        {
            get { return this._createdtxt; }
            set { this.SetProperty(ref this._createdtxt, value); }
        }

        private string _Colour = string.Empty;
        public string Colour
        {
            get { return this._Colour; }
            set { this.SetProperty(ref this._Colour, value); }
        }
        private string _bgColour = string.Empty;
        public string bgColour
        {
            get { return this._bgColour; }
            set { this.SetProperty(ref this._bgColour, value); }
        }
        private string _createdontwo = string.Empty;
        public string CreatedOnTwo
        {
            get { return this._createdontwo; }
            set { this.SetProperty(ref this._createdontwo, value); }
        }
        private string _createdtxttwo = string.Empty;
        public string CreatedTxtTwo
        {
            get { return this._createdtxttwo; }
            set { this.SetProperty(ref this._createdtxttwo, value); }
        }

        private string _currentStatus = string.Empty;
        public string CurrentStatus
        {
            get { return this._currentStatus; }
            set { this.SetProperty(ref this._currentStatus, value); }
        }
    }

    /// <summary>
    /// Generic group data model.
    /// </summary>
    public class SampleDataGroup : SampleDataCommon
    {
        public SampleDataGroup(String uniqueId, String title, String subtitle, String imagePath, String description)
            : base(uniqueId, title, subtitle, imagePath, description)
        {
        }

        private ObservableCollection<SampleDataItem> _items = new ObservableCollection<SampleDataItem>();
        public ObservableCollection<SampleDataItem> Items
        {
            get { return this._items; }
        }
        
        public IEnumerable<SampleDataItem> TopItems
        {
            // Provides a subset of the full items collection to bind to from a GroupedItemsPage
            // for two reasons: GridView will not virtualize large items collections, and it
            // improves the user experience when browsing through groups with large numbers of
            // items.
            //
            // A maximum of 12 items are displayed because it results in filled grid columns
            // whether there are 1, 2, 3, 4, or 6 rows displayed
            get { return this._items.Take(12); }
        }
    }

    /// <summary>
    /// Creates a collection of groups and items with hard-coded content.
    /// </summary>
    public sealed class SampleDataSource
    {
        private static SampleDataSource _sampleDataSource = new SampleDataSource();

        private ObservableCollection<SampleDataGroup> _allGroups = new ObservableCollection<SampleDataGroup>();
        public ObservableCollection<SampleDataGroup> AllGroups
        {
            get { return this._allGroups; }
        }

        public static IEnumerable<SampleDataGroup> GetGroups(string uniqueId)
        {
            if (!uniqueId.Equals("AllGroups")) throw new ArgumentException("Only 'AllGroups' is supported as a collection of groups");
            
            return _sampleDataSource.AllGroups;
        }

        public static SampleDataGroup GetGroup(string uniqueId)
        {
            // Simple linear search is acceptable for small data sets
            var matches = _sampleDataSource.AllGroups.Where((group) => group.UniqueId.Equals(uniqueId));
            if (matches.Count() == 1) return matches.First();
            return null;
        }

        public static SampleDataItem GetItem(string uniqueId)
        {
            // Simple linear search is acceptable for small data sets
            var matches = _sampleDataSource.AllGroups.SelectMany(group => group.Items).Where((item) => item.UniqueId.Equals(uniqueId));
            if (matches.Count() == 1) return matches.First();
            return null;
        }

        public SampleDataSource()
        {
           // String ITEM_CONTENT = String.Format("");

            var group1 = new SampleDataGroup("Group-1",
                    "Ways & Directions",
                    "Ways & Directions",
                    "Assets/Images/10.jpg",
                    "Modern life is full of hassles, deadlines, frustrations, and demands. For many people, stress is so commonplace that it has become a way of life. Stress isn’t always bad. In small doses, it can help you perform under pressure and motivate you to do your best.");
            group1.Items.Add(new SampleDataItem("Group-1-Item-1",
                    "What is stress?",
                    "Stress is a normal physical response to events that make you feel threatened or upset your balance in some way. When you sense danger – whether it’s real or imagined – the body's defenses kick into high gear in a rapid, automatic process known as the “fight-or-flight” reaction, or the stress response.",
                    "Assets/DarkGray.png",
					"",            
                    "Details:\n\nStress is a normal physical response to events that make you feel threatened or upset your balance in some way. When you sense danger – whether it’s real or imagined – the body's defenses kick into high gear in a rapid, automatic process known as the “fight-or-flight” reaction, or the stress response. The stress response is the body’s way of protecting you. When working properly, it helps you stay focused, energetic, and alert. In emergency situations, stress can save your life – giving you extra strength to defend yourself, for example, or spurring you to slam on the brakes to avoid an accident.\n\nThe stress response also helps you rise to meet challenges. Stress is what keeps you on your toes during a presentation at work, sharpens your concentration when you’re attempting the game-winning free throw, or drives you to study for an exam when you'd rather be watching TV. But beyond a certain point, stress stops being helpful and starts causing major damage to your health, your mood, your productivity, your relationships, and your quality of life.",
                    group1) { CreatedOn = "Group", CreatedTxt = "Ways & Directions", CreatedOnTwo = "Item", CreatedTxtTwo = "What is stress?", bgColour = "#6495ED", Image = new BitmapImage(new Uri(new Uri("ms-appx:///"), "Assets/Images/11.jpg")), CurrentStatus = "Stress & Stress Control" });
            group1.Items.Add(new SampleDataItem("Group-1-Item-2",
                     "Causes of stress",
                     "The situations and pressures that cause stress are known as stressors. We usually think of stressors as being negative, such as an exhausting work schedule or a rocky relationship.",
                     "Assets/DarkGray.png",
                     "",
                     "Details:\n\nThe situations and pressures that cause stress are known as stressors. We usually think of stressors as being negative, such as an exhausting work schedule or a rocky relationship. However, anything that puts high demands on you or forces you to adjust can be stressful. This includes positive events such as getting married, buying a house, going to college, or receiving a promotion. What causes stress depends, at least in part, on your perception of it. Something that's stressful to you may not faze someone else; they may even enjoy it. For example, your morning commute may make you anxious and tense because you worry that traffic will make you late. Others, however, may find the trip relaxing because they allow more than enough time and enjoy listening to music while they drive.",
                     group1) { CreatedOn = "Group", CreatedTxt = "Ways & Directions", CreatedOnTwo = "Item", CreatedTxtTwo = "Causes of stress", bgColour = "#DAA520", Image = new BitmapImage(new Uri(new Uri("ms-appx:///"), "Assets/Images/12.jpg")), CurrentStatus = "Stress & Stress Control" });
			group1.Items.Add(new SampleDataItem("Group-1-Item-3",
                     "Effects of chronic stress",
                     "The body doesn’t distinguish between physical and psychological threats. When you’re stressed over a busy schedule, an argument with a friend, a traffic jam, or a mountain of bills, your body reacts just as strongly as if you were facing a life-or-death situation.",
                     "Assets/DarkGray.png",
                     "",
                     "Details:\n\nThe body doesn’t distinguish between physical and psychological threats. When you’re stressed over a busy schedule, an argument with a friend, a traffic jam, or a mountain of bills, your body reacts just as strongly as if you were facing a life-or-death situation. If you have a lot of responsibilities and worries, your emergency stress response may be “on” most of the time. The more your body’s stress system is activated, the harder it is to shut off. Long-term exposure to stress can lead to serious health problems. Chronic stress disrupts nearly every system in your body. It can raise blood pressure, suppress the immune system, increase the risk of heart attack and stroke, contribute to infertility, and speed up the aging process. Long-term stress can even rewire the brain, leaving you more vulnerable to anxiety and depression.",
                     group1) { CreatedOn = "Group", CreatedTxt = "Ways & Directions", CreatedOnTwo = "Item", CreatedTxtTwo = "Effects of chronic stress", bgColour = "#DAA520", Image = new BitmapImage(new Uri(new Uri("ms-appx:///"), "Assets/Images/13.jpg")), CurrentStatus = "Stress & Stress Control" });
			group1.Items.Add(new SampleDataItem("Group-1-Item-4",
                     "Dealing with stress and its symptoms",
                     "While unchecked stress is undeniably damaging, there are many things you can do to reduce its impact and cope with symptoms. Learn how to manage stres.",
                     "Assets/DarkGray.png",
                     "",
                     "Details:\n\nWhile unchecked stress is undeniably damaging, there are many things you can do to reduce its impact and cope with symptoms. Learn how to manage stres. You may feel like the stress in your life is out of your control, but you can always control the way you respond. Managing stress is all about taking charge: taking charge of your thoughts, your emotions, your schedule, your environment, and the way you deal with problems. Stress management involves changing the stressful situation when you can, changing your reaction when you can’t, taking care of yourself, and making time for rest and relaxation. Learn how to relax\n\nYou can’t completely eliminate stress from your life, but you can control how much it affects you. Relaxation techniques such as yoga, meditation, and deep breathing activate the body’s relaxation response, a state of restfulness that is the opposite of the stress response. When practiced regularly, these activities lead to a reduction in your everyday stress levels and a boost in your feelings of joy and serenity. They also increase your ability to stay calm and collected under pressure.",
                     group1) { CreatedOn = "Group", CreatedTxt = "Ways & Directions", CreatedOnTwo = "Item", CreatedTxtTwo = "Dealing with stress and its symptoms", bgColour = "#DAA520", Image = new BitmapImage(new Uri(new Uri("ms-appx:///"), "Assets/Images/14.jpg")), CurrentStatus = "Stress & Stress Control" });
			group1.Items.Add(new SampleDataItem("Group-1-Item-5",
                     "Stress Management",
                     "Stress management starts with identifying the sources of stress in your life. This isn’t as easy as it sounds. Your true sources of stress aren’t always obvious, and it’s all too easy to overlook your own stress-inducing thoughts, feelings, and behaviors.",
                     "Assets/DarkGray.png",
                     "",
                     "Details:\n\nStress management starts with identifying the sources of stress in your life. This isn’t as easy as it sounds. Your true sources of stress aren’t always obvious, and it’s all too easy to overlook your own stress-inducing thoughts, feelings, and behaviors. Sure, you may know that you’re constantly worried about work deadlines. But maybe it’s your procrastination, rather than the actual job demands, that leads to deadline stress. To identify your true sources of stress, look closely at your habits, attitude, and excuses: Do you explain away stress as temporary (“I just have a million things going on right now”) even though you can’t remember the last time you took a breather? Do you define stress as an integral part of your work or home life (“Things are always crazy around here”) or as a part of your personality (“I have a lot of nervous energy, that’s all”).Do you blame your stress on other people or outside events, or view it as entirely normal and unexceptional? Until you accept responsibility for the role you play in creating or maintaining it, your stress level will remain outside your control.",
                     group1) { CreatedOn = "Group", CreatedTxt = "Ways & Directions", CreatedOnTwo = "Item", CreatedTxtTwo = "Stress Management", bgColour = "#DAA520", Image = new BitmapImage(new Uri(new Uri("ms-appx:///"), "Assets/Images/15.jpg")), CurrentStatus = "Stress & Stress Control" });
			group1.Items.Add(new SampleDataItem("Group-1-Item-6",
                     "Become a Virtual Assistant",
                     "Every small businessman would love to hire a full time assistant to take care of the little things, but many simply can’t afford one.",
                     "Assets/DarkGray.png",
                     "",
                     "Details:\n\nEvery small businessman would love to hire a full time assistant to take care of the little things, but many simply can’t afford one. Thanks to the Internet, though, they can now hire part time assistants who work for a whole host of clients, and all at a much lower cost than a full time staff member. If you work from home this may be a perfect opportunity to make a consistent income. Virtual assistants can earn $20 an hour in return for booking travel tickets, interacting with clients and dealing with the daily needs of small businesses.",
                     group1) { CreatedOn = "Group", CreatedTxt = "Ways & Directions", CreatedOnTwo = "Item", CreatedTxtTwo = "Become a Virtual Assistant", bgColour = "#DAA520", Image = new BitmapImage(new Uri(new Uri("ms-appx:///"), "Assets/Images/16.jpg")), CurrentStatus = "Stress & Stress Control" });
			group1.Items.Add(new SampleDataItem("Group-1-Item-7",
                     "Avoid unnecessary stress",
                     "Not all stress can be avoided, and it’s not healthy to avoid a situation that needs to be addressed. You may be surprised, however, by the number of stressors in your life that you can eliminate.",
                     "Assets/DarkGray.png",
                     "",
                     "Details:\n\nNot all stress can be avoided, and it’s not healthy to avoid a situation that needs to be addressed. You may be surprised, however, by the number of stressors in your life that you can eliminate. Learn how to say “no” – Know your limits and stick to them. Whether in your personal or professional life, refuse to accept added responsibilities when you’re close to reaching them. Taking on more than you can handle is a surefire recipe for stress.\n\nAvoid people who stress you out – If someone consistently causes stress in your life and you can’t turn the relationship around, limit the amount of time you spend with that person or end the relationship entirely. Take control of your environment – If the evening news makes you anxious, turn the TV off. If traffic’s got you tense, take a longer but less-traveled route. If going to the market is an unpleasant chore, do your grocery shopping online. Avoid hot-button topics – If you get upset over religion or politics, cross them off your conversation list. If you repeatedly argue about the same subject with the same people, stop bringing it up or excuse yourself when it’s the topic of discussion. Pare down your to-do list – Analyze your schedule, responsibilities, and daily tasks. If you’ve got too much on your plate, distinguish between the “shoulds” and the “musts.” Drop tasks that aren’t truly necessary to the bottom of the list or eliminate them entirely.",
                     group1) { CreatedOn = "Group", CreatedTxt = "Ways & Directions", CreatedOnTwo = "Item", CreatedTxtTwo = "Avoid unnecessary stress", bgColour = "#DAA520", Image = new BitmapImage(new Uri(new Uri("ms-appx:///"), "Assets/Images/17.jpg")), CurrentStatus = "Stress & Stress Control" });
			group1.Items.Add(new SampleDataItem("Group-1-Item-8",
                     "Alter the situation",
                     "If you can’t avoid a stressful situation, try to alter it. Figure out what you can do to change things so the problem doesn’t present itself in the future. Often, this involves changing the way you communicate and operate in your daily life. Express your feelings instead of bottling them up.",
                     "Assets/DarkGray.png",
                     "",
                     "Details:\n\nIf you can’t avoid a stressful situation, try to alter it. Figure out what you can do to change things so the problem doesn’t present itself in the future. Often, this involves changing the way you communicate and operate in your daily life. Express your feelings instead of bottling them up. If something or someone is bothering you, communicate your concerns in an open and respectful way. If you don’t voice your feelings, resentment will build and the situation will likely remain the same. Be willing to compromise. When you ask someone to change their behavior, be willing to do the same. If you both are willing to bend at least a little, you’ll have a good chance of finding a happy middle ground.\n\nBe more assertive. Don’t take a backseat in your own life. Deal with problems head on, doing your best to anticipate and prevent them. If you’ve got an exam to study for and your chatty roommate just got home, say up front that you only have five minutes to talk. Manage your time better. Poor time management can cause a lot of stress. When you’re stretched too thin and running behind, it’s hard to stay calm and focused. But if you plan ahead and make sure you don’t overextend yourself, you can alter the amount of stress you’re under.",
                     group1) { CreatedOn = "Group", CreatedTxt = "Ways & Directions", CreatedOnTwo = "Item", CreatedTxtTwo = "Alter the situation", bgColour = "#DAA520", Image = new BitmapImage(new Uri(new Uri("ms-appx:///"), "Assets/Images/18.jpg")), CurrentStatus = "Stress & Stress Control" });
			group1.Items.Add(new SampleDataItem("Group-1-Item-9",
                     "Adapt to the stressor",
                     "If you can’t change the stressor, change yourself. You can adapt to stressful situations and regain your sense of control by changing your expectations and attitude.",
                     "Assets/DarkGray.png",
                     "",
                     "Details:\n\nIf you can’t change the stressor, change yourself. You can adapt to stressful situations and regain your sense of control by changing your expectations and attitude. Reframe problems. Try to view stressful situations from a more positive perspective. Rather than fuming about a traffic jam, look at it as an opportunity to pause and regroup, listen to your favorite radio station, or enjoy some alone time. Look at the big picture. Take perspective of the stressful situation. Ask yourself how important it will be in the long run. Will it matter in a month? A year? Is it really worth getting upset over? If the answer is no, focus your time and energy elsewhere. Adjust your standards. Perfectionism is a major source of avoidable stress. Stop setting yourself up for failure by demanding perfection. Set reasonable standards for yourself and others, and learn to be okay with “good enough.” \n\nFocus on the positive. When stress is getting you down, take a moment to reflect on all the things you appreciate in your life, including your own positive qualities and gifts. This simple strategy can help you keep things in perspective.",
                     group1) { CreatedOn = "Group", CreatedTxt = "Ways & Directions", CreatedOnTwo = "Item", CreatedTxtTwo = "Adapt to the stressor", bgColour = "#DAA520", Image = new BitmapImage(new Uri(new Uri("ms-appx:///"), "Assets/Images/19.jpg")), CurrentStatus = "Stress & Stress Control" });
			group1.Items.Add(new SampleDataItem("Group-1-Item-10",
                     "Relaxation Techniques",
                     "Stress is necessary for life. You need stress for creativity, learning, and your very survival. Stress is only harmful when it becomes overwhelming and interrupts the healthy state of equilibrium that your nervous system needs to remain in balance.",
                     "Assets/DarkGray.png",
                     "",
                     "Details:\n\nStress is necessary for life. You need stress for creativity, learning, and your very survival. Stress is only harmful when it becomes overwhelming and interrupts the healthy state of equilibrium that your nervous system needs to remain in balance. Unfortunately, overwhelming stress has become an increasingly common characteristic of contemporary life. When stressors throw your nervous system out of balance, relaxation techniques can bring it back into a balanced state by producing the relaxation response, a state of deep calmness that is the polar opposite of the stress response. \n\nWhen stress overwhelms your nervous system your body is flooded with chemicals that prepare you for “fight or flight”. While the stress response can be lifesaving in emergency situations where you need to act quickly, it wears your body down when constantly activated by the stresses of everyday life. The relaxation response puts the brakes on this heightened state of readiness and brings your body and mind back into a state of equilibrium.",
                     group1) { CreatedOn = "Group", CreatedTxt = "Ways & Directions", CreatedOnTwo = "Item", CreatedTxtTwo = "Relaxation Techniques", bgColour = "#DAA520", Image = new BitmapImage(new Uri(new Uri("ms-appx:///"), "Assets/Images/20.jpg")), CurrentStatus = "Stress & Stress Control" });
					 
            this.AllGroups.Add(group1);


			
			
         
        }
    }
}
