namespace Server.Entities
{
	public class Product
	{
        //таблица из полей:
		public long Id { get; init; } //Наличие Id обязательно
        public string Name { get; set; }
        public int Price { get; set; }
        public string Description { get; set; }
        public string LinkImg { get; set; }
		public string LinkFullDescription { get; set; }
        public int Category { get; set; }
    }
}
