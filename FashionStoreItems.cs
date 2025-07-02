using LitJson;
using SLAM.Webservices;
using System.Collections.Generic;

public class FashionStoreItems
{
    public static ShopItemData[] All => CombineAll();

    private static ShopItemData[] CombineAll()
    {
        var list = new List<ShopItemData>();
        list.AddRange(Boy_Dog_Items);
        list.AddRange(Boy_Duck_Items);
        list.AddRange(Boy_Cat_Items);
        list.AddRange(Girl_Dog_Items);
        list.AddRange(Girl_Duck_Items);
        list.AddRange(Girl_Cat_Items);
        return list.ToArray();
    }

    public static ShopItemData[] Boy_Dog_Items = new ShopItemData[]
    {
        new ShopItemData
        {
            Id = 2,
            GUID = "31ea5849-b3b6-47e9-b6b8-0a606cee53ac",
            Title = "",
            Description = "",
            Price = 0,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 3,
            GUID = "93c3cb8b-6bf0-4645-a075-474c0fecdeab",
            Title = "",
            Description = "",
            Price = 0,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 4,
            GUID = "40511d1d-5b31-4445-9b9c-6c284cc3a290",
            Title = "",
            Description = "",
            Price = 0,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 5,
            GUID = "ab453e65-d8e2-442b-abe9-a74efcaa44ca",
            Title = "",
            Description = "",
            Price = 0,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 6,
            GUID = "b473441f-d97c-4008-a832-e51a861cff47",
            Title = "",
            Description = "",
            Price = 0,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 7,
            GUID = "5fec4abb-9179-4c65-88bf-99ec39220302",
            Title = "",
            Description = "",
            Price = 0,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 8,
            GUID = "9dfb5a81-0e92-479e-abaa-ed0b9f7fe001",
            Title = "",
            Description = "",
            Price = 0,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 9,
            GUID = "b46bd88b-b6cf-4bc7-be8b-8d3f83a66a1c",
            Title = "",
            Description = "",
            Price = 0,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 10,
            GUID = "d3167c5e-42f6-43fe-9a1a-6c4045270499",
            Title = "",
            Description = "",
            Price = 0,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 11,
            GUID = "f997b86a-34d3-4b81-98f2-1928b8a69599",
            Title = "",
            Description = "",
            Price = 0,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 12,
            GUID = "32599b5a-9c8a-4283-8439-f3dd85fd465b",
            Title = "",
            Description = "",
            Price = 0,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 13,
            GUID = "ee1bc46f-f2df-4701-a48d-5035bfa91897",
            Title = "",
            Description = "",
            Price = 40,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 14,
            GUID = "9f100b25-58a3-44bc-bbfe-9eb851857e30",
            Title = "",
            Description = "",
            Price = 40,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 15,
            GUID = "af3fe207-7165-45ad-98dc-7de4fdcc6926",
            Title = "",
            Description = "",
            Price = 40,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 16,
            GUID = "43a1413c-370e-4515-bd9d-c9b0afa54402",
            Title = "",
            Description = "",
            Price = 25,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 17,
            GUID = "d8bc1354-cb4d-4426-9104-6397ef40dd1f",
            Title = "",
            Description = "",
            Price = 25,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 18,
            GUID = "9948e3e5-8742-4352-a844-5bb5d3f41ab2",
            Title = "",
            Description = "",
            Price = 25,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 19,
            GUID = "bc7a2175-eba0-48e1-93e4-8ce5eb54473b",
            Title = "",
            Description = "",
            Price = 25,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 20,
            GUID = "94bbe51f-1980-4024-b466-5a66551fe2e5",
            Title = "",
            Description = "",
            Price = 40,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 21,
            GUID = "a3baae4c-6969-4b15-bd1d-810b28bf6c98",
            Title = "",
            Description = "",
            Price = 40,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 22,
            GUID = "1a5a79e5-8f3d-4787-8d95-5a874d78f9d3",
            Title = "",
            Description = "",
            Price = 40,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 23,
            GUID = "ef2e6333-6b6f-4649-9a4f-45d7ad60d673",
            Title = "",
            Description = "",
            Price = 40,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 24,
            GUID = "9df253d3-8327-46a4-ba84-fa0ad4e41546",
            Title = "",
            Description = "",
            Price = 10,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 25,
            GUID = "9ef6db59-d651-4947-8f42-20b6ba7e448a",
            Title = "",
            Description = "",
            Price = 10,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 26,
            GUID = "a7fed51a-03d4-4f31-b31d-47ab17b9f67a",
            Title = "",
            Description = "",
            Price = 10,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 27,
            GUID = "a353a7c2-3646-4725-9e89-cf7e153a0c38",
            Title = "",
            Description = "",
            Price = 10,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 28,
            GUID = "f44784fb-bbcb-4ea1-9ba0-ac6655fbc105",
            Title = "",
            Description = "",
            Price = 10,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 29,
            GUID = "4b53da93-7164-430b-9c4e-087ed8a4f3bc",
            Title = "",
            Description = "",
            Price = 10,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 30,
            GUID = "6a072c78-dff7-4932-b4a1-843ed69c7421",
            Title = "",
            Description = "",
            Price = 10,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 31,
            GUID = "be584351-e045-4a2f-8d65-a521e822837a",
            Title = "",
            Description = "",
            Price = 10,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 32,
            GUID = "bcf7f892-171b-4b7c-909c-2590309d6ec4",
            Title = "",
            Description = "",
            Price = 10,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 33,
            GUID = "0d5c5f52-b00c-45a5-a669-583fab016c3b",
            Title = "",
            Description = "",
            Price = 10,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 34,
            GUID = "4fc766d1-2ad2-4497-96ce-79f26bc2faf4",
            Title = "",
            Description = "",
            Price = 10,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 35,
            GUID = "690926cb-588c-4dea-af75-da5d2e0aba84",
            Title = "",
            Description = "",
            Price = 10,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 36,
            GUID = "941104c8-b7ae-47b1-a909-0a60e1564aa4",
            Title = "",
            Description = "",
            Price = 10,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 37,
            GUID = "58804675-8f03-4711-9e31-b23a4b3443bf",
            Title = "",
            Description = "",
            Price = 10,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 38,
            GUID = "1188eb9e-8029-43a2-9445-003afacaaf14",
            Title = "",
            Description = "",
            Price = 10,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 39,
            GUID = "f3b4db58-3d72-4cbe-8d3e-8bbef0d12062",
            Title = "",
            Description = "",
            Price = 10,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 40,
            GUID = "1c4a112e-eaa8-4d9f-9657-cf30ae16e8ab",
            Title = "",
            Description = "",
            Price = 10,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 41,
            GUID = "fb9d928c-948d-4423-9645-ccfc0f3b65d8",
            Title = "",
            Description = "",
            Price = 10,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 42,
            GUID = "f89b5a08-a848-4fd5-9a58-b6cf0eb1a2d8",
            Title = "",
            Description = "",
            Price = 10,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 43,
            GUID = "39d4e474-94c8-40aa-8231-e2564a7df06f",
            Title = "",
            Description = "",
            Price = 10,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 44,
            GUID = "105dc699-b9bd-498c-8b85-5d7f7529b521",
            Title = "",
            Description = "",
            Price = 10,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 45,
            GUID = "6e5ca3b8-bbef-4873-8d00-495aee159943",
            Title = "",
            Description = "",
            Price = 10,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 46,
            GUID = "6126fc0a-2486-4c68-82be-30a87e9fcddf",
            Title = "",
            Description = "",
            Price = 10,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 47,
            GUID = "8fad423f-2d9e-4239-ab0b-c412860db723",
            Title = "",
            Description = "",
            Price = 10,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 48,
            GUID = "dfd21bb1-7281-45c8-b45e-66436f6a94ef",
            Title = "",
            Description = "",
            Price = 10,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 49,
            GUID = "8d167b77-1eae-42c3-85e2-cb0d4212ebf3",
            Title = "",
            Description = "",
            Price = 10,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 50,
            GUID = "c66915be-4b9f-41d4-8ffd-7d6b2bf59b9d",
            Title = "",
            Description = "",
            Price = 10,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 51,
            GUID = "fff3d986-c267-4a61-a4b8-4b7fca4fb97a",
            Title = "",
            Description = "",
            Price = 10,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 52,
            GUID = "00fec8b9-d6f7-41de-bc16-af0d2e42c289",
            Title = "",
            Description = "",
            Price = 999999999,
            VisibleInShop = false,
        },
        new ShopItemData
        {
            Id = 53,
            GUID = "3618f0d0-a524-4910-a13c-695eb3532edd",
            Title = "",
            Description = "",
            Price = 999999999,
            VisibleInShop = false,
        },
        new ShopItemData
        {
            Id = 54,
            GUID = "07a395f5-6ef3-45fb-9c22-a7410082d7c2",
            Title = "",
            Description = "",
            Price = 10,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 55,
            GUID = "9bb559eb-b3cc-423b-a01a-e67e8d6b952e",
            Title = "",
            Description = "",
            Price = 10,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 56,
            GUID = "bd1bf716-8345-48b9-a885-976dec4e60a3",
            Title = "",
            Description = "",
            Price = 10,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 57,
            GUID = "b5170525-9d1e-48a2-9b22-54c51f55f798",
            Title = "",
            Description = "",
            Price = 10,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 58,
            GUID = "547089ff-fc3f-4761-b575-4cf0028168e7",
            Title = "",
            Description = "",
            Price = 25,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 59,
            GUID = "89c471de-2029-4732-9cb3-f61ede704500",
            Title = "",
            Description = "",
            Price = 25,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 60,
            GUID = "06f6c189-8b84-4d79-bbf4-dca6f7213596",
            Title = "",
            Description = "",
            Price = 25,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 61,
            GUID = "d54bbbf2-433c-4d74-a898-cfbee86739f5",
            Title = "",
            Description = "",
            Price = 25,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 62,
            GUID = "f79e257c-8ea8-4fa2-9eb9-b3a33cc6c2a3",
            Title = "",
            Description = "",
            Price = 25,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 63,
            GUID = "f45178ab-4582-4b05-9ebd-e1a8e4b66658",
            Title = "",
            Description = "",
            Price = 25,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 64,
            GUID = "c51fbedb-9001-4be3-a137-2bac3dc65974",
            Title = "",
            Description = "",
            Price = 25,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 65,
            GUID = "84e88352-49c6-4ffe-8e2c-12cec6323789",
            Title = "",
            Description = "",
            Price = 25,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 66,
            GUID = "441f852e-ac74-48bd-8d22-6c7e528aa299",
            Title = "",
            Description = "",
            Price = 25,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 67,
            GUID = "0656e0d1-6c4d-42cf-be11-e9102979d355",
            Title = "",
            Description = "",
            Price = 25,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 68,
            GUID = "d41fece1-36b7-4a32-9369-0851412d4c19",
            Title = "",
            Description = "",
            Price = 25,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 69,
            GUID = "837322cf-6166-4c74-8019-0852b617dd8c",
            Title = "",
            Description = "",
            Price = 25,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 70,
            GUID = "8ceb87c7-6874-4a96-a7f1-74a23288674e",
            Title = "",
            Description = "",
            Price = 40,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 71,
            GUID = "0581137f-1851-4da3-b0e5-18be364046ec",
            Title = "",
            Description = "",
            Price = 40,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 72,
            GUID = "dbcacdcd-5425-4cbb-b4e7-b3a24af4a5cb",
            Title = "",
            Description = "",
            Price = 40,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 73,
            GUID = "6afabf8f-5e3e-4d9f-8fa1-2209e58cb25c",
            Title = "",
            Description = "",
            Price = 40,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 74,
            GUID = "c1291d90-f141-449c-800e-80a3f715f33f",
            Title = "",
            Description = "",
            Price = 0,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 75,
            GUID = "b8d48732-724f-4f7d-a546-346e2a6f992c",
            Title = "",
            Description = "",
            Price = 25,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 76,
            GUID = "880f2087-711e-45c0-9046-c8d897bf676f",
            Title = "",
            Description = "",
            Price = 25,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 77,
            GUID = "f048c949-a16a-44e8-8969-e4ed42d38b23",
            Title = "",
            Description = "",
            Price = 25,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 78,
            GUID = "9c675a13-b379-455a-a2a5-e92b87af1f26",
            Title = "",
            Description = "",
            Price = 25,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 79,
            GUID = "55247391-9c0d-46c2-a358-795cf2f2f4fa",
            Title = "",
            Description = "",
            Price = 25,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 80,
            GUID = "04dc3c23-fabb-44db-9efb-68169e2f1f82",
            Title = "",
            Description = "",
            Price = 25,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 81,
            GUID = "78f18d15-689e-4572-ad96-9b9e9833f8d8",
            Title = "",
            Description = "",
            Price = 25,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 82,
            GUID = "0181be13-32f2-4d02-8287-46732271233c",
            Title = "",
            Description = "",
            Price = 10,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 83,
            GUID = "175f94bb-5275-4022-bc50-0c9d6471b59d",
            Title = "",
            Description = "",
            Price = 10,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 84,
            GUID = "8c168f68-e49f-4996-a65f-58274a779472",
            Title = "",
            Description = "",
            Price = 10,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 85,
            GUID = "f3761816-23cd-4fce-b5d7-6879e6e45c3e",
            Title = "",
            Description = "",
            Price = 10,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 86,
            GUID = "b1da6b2a-f8a6-40e0-800f-8f6078ed1175",
            Title = "",
            Description = "",
            Price = 50,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 87,
            GUID = "0f112f17-7dd7-4c44-88e5-d055f00e570c",
            Title = "",
            Description = "",
            Price = 50,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 88,
            GUID = "ed154761-6c6e-487f-bdc7-c081eb2efae6",
            Title = "",
            Description = "",
            Price = 50,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 89,
            GUID = "5f46abbc-5f7d-447b-bcc5-61b4cb277ef4",
            Title = "",
            Description = "",
            Price = 50,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 90,
            GUID = "1c68e30f-0257-4873-a091-de097aee3b32",
            Title = "",
            Description = "",
            Price = 50,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 91,
            GUID = "e1408a41-f884-4aed-bdd7-a16b53a15068",
            Title = "",
            Description = "",
            Price = 50,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 92,
            GUID = "5879fa46-d761-4823-8ffb-2f2fed400e40",
            Title = "",
            Description = "",
            Price = 50,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 93,
            GUID = "931c40b0-ccf4-48da-8721-76844a97c021",
            Title = "",
            Description = "",
            Price = 50,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 94,
            GUID = "2f7bc7b9-039c-4f93-b4cd-a6b77ebb9ac9",
            Title = "",
            Description = "",
            Price = 50,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 95,
            GUID = "015efdad-a657-4ff9-bcf2-7d5924623355",
            Title = "",
            Description = "",
            Price = 50,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 96,
            GUID = "e4578bcf-f29c-4688-838f-2c520ab5ce07",
            Title = "",
            Description = "",
            Price = 50,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 97,
            GUID = "6cdd31da-68ab-43d9-bf72-91d61d6794f5",
            Title = "",
            Description = "",
            Price = 50,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 98,
            GUID = "d40d736d-e224-4d34-b102-793adee32d95",
            Title = "",
            Description = "",
            Price = 0,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 99,
            GUID = "e6c0244f-6ff7-4585-a378-67e9220ff608",
            Title = "",
            Description = "",
            Price = 20,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 100,
            GUID = "3ede1684-441b-4378-97b4-5ed4405a925a",
            Title = "",
            Description = "",
            Price = 20,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 101,
            GUID = "ab5827fa-44c2-4547-8832-0dd5b7834f74",
            Title = "",
            Description = "",
            Price = 20,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 102,
            GUID = "5371f875-2b1c-495e-9909-0116f32857b7",
            Title = "",
            Description = "",
            Price = 30,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 103,
            GUID = "63e9aa79-f54e-4abf-9564-3ee77ae8681b",
            Title = "",
            Description = "",
            Price = 30,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 104,
            GUID = "ee160a1e-086c-4ef9-9c27-59e5f299c0ad",
            Title = "",
            Description = "",
            Price = 30,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 105,
            GUID = "28712854-2bb7-4381-bf90-8b02322d3a56",
            Title = "",
            Description = "",
            Price = 30,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 106,
            GUID = "d145b261-f34f-4960-a95d-eb2f81c48e20",
            Title = "",
            Description = "",
            Price = 30,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 107,
            GUID = "c767621e-67b2-4fa1-8ebb-93a54c363ef8",
            Title = "",
            Description = "",
            Price = 30,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 108,
            GUID = "895e7a38-2178-4b7f-b31f-17e4cfa4a3c6",
            Title = "",
            Description = "",
            Price = 30,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 109,
            GUID = "0c1aa825-6c6e-4d17-8907-a65b78097b26",
            Title = "",
            Description = "",
            Price = 30,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 110,
            GUID = "8144816f-2ecb-48eb-bc40-382a7fff7c82",
            Title = "",
            Description = "",
            Price = 30,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 111,
            GUID = "3b5faf0e-7bb7-469b-aa3a-6539ca5fd09e",
            Title = "",
            Description = "",
            Price = 30,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 112,
            GUID = "06a9ff85-40d5-4800-b121-8822004d332f",
            Title = "",
            Description = "",
            Price = 30,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 113,
            GUID = "3ababa4b-373b-4003-8f36-8dfc93b03f0a",
            Title = "",
            Description = "",
            Price = 30,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 114,
            GUID = "d6d168e4-e2d1-4532-8556-14d1cbf3083e",
            Title = "",
            Description = "",
            Price = 30,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 115,
            GUID = "9f0ed8cb-850f-4336-befa-22d5af25894e",
            Title = "",
            Description = "",
            Price = 30,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 116,
            GUID = "ec2307fa-a6ec-483e-bb37-a781829be242",
            Title = "",
            Description = "",
            Price = 30,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 117,
            GUID = "c886951a-68d2-45b7-8d4d-1d69c15917e9",
            Title = "",
            Description = "",
            Price = 30,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 118,
            GUID = "79e76657-f209-458d-b3dc-bf26de39f7f6",
            Title = "",
            Description = "",
            Price = 40,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 119,
            GUID = "3babf459-18ca-4df8-8e6b-9bea4bf191e6",
            Title = "",
            Description = "",
            Price = 40,
            VisibleInShop = true,
        }
    };

    public static ShopItemData[] Boy_Duck_Items = new ShopItemData[]
    {
        new ShopItemData
        {
            Id = 120,
            GUID = "2f90680a-a494-4f2a-91d4-5702b7fc8265",
            Title = "",
            Description = "",
            Price = 0,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 121,
            GUID = "8bead4c1-8e3e-4041-9854-d987d124e6b7",
            Title = "",
            Description = "",
            Price = 0,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 122,
            GUID = "1afa0c9e-7e6a-43d8-ab79-bd453b817104",
            Title = "",
            Description = "",
            Price = 0,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 123,
            GUID = "af41ea3a-2e00-415d-9704-83080577d2a8",
            Title = "",
            Description = "",
            Price = 0,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 124,
            GUID = "8016daf6-edd3-43b1-b6cf-33ad7d223882",
            Title = "",
            Description = "",
            Price = 0,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 125,
            GUID = "6a31ea80-442c-4900-a2ec-8af6ba6231a5",
            Title = "",
            Description = "",
            Price = 0,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 126,
            GUID = "f85acdd6-5bbf-4717-8ee8-23434017ed7b",
            Title = "",
            Description = "",
            Price = 0,
            VisibleInShop = false,
        },
        new ShopItemData
        {
            Id = 127,
            GUID = "f1e25181-1c4e-4aa1-9aa6-2ab4e3657a9a",
            Title = "",
            Description = "",
            Price = 0,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 128,
            GUID = "a2709a72-a035-4274-be2f-013d979f5271",
            Title = "",
            Description = "",
            Price = 0,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 129,
            GUID = "9e35b4c6-f212-4be5-8662-9b1106f3bf6a",
            Title = "",
            Description = "",
            Price = 40,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 130,
            GUID = "6bfc9f6f-d445-48ec-a716-ca3f9f4c95ec",
            Title = "",
            Description = "",
            Price = 40,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 131,
            GUID = "0e507f2b-9403-4d05-b5fc-ff99a0b8ec7e",
            Title = "",
            Description = "",
            Price = 40,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 132,
            GUID = "ec996d01-a744-4053-9f7e-49b55d4d33ea",
            Title = "",
            Description = "",
            Price = 25,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 133,
            GUID = "53731464-373b-43ce-9833-daad3cb9cf9c",
            Title = "",
            Description = "",
            Price = 25,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 134,
            GUID = "4da15565-568f-415d-89af-8979ba7e706d",
            Title = "",
            Description = "",
            Price = 25,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 135,
            GUID = "093d2ef4-0038-4799-9d19-e84edbf824f3",
            Title = "",
            Description = "",
            Price = 25,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 136,
            GUID = "b72345e0-5358-4d90-a798-f9fd295a8bb5",
            Title = "",
            Description = "",
            Price = 40,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 137,
            GUID = "8f5c4678-7bf0-4fcf-892b-0ac147a58ac5",
            Title = "",
            Description = "",
            Price = 40,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 138,
            GUID = "212b898e-7488-49b5-8d17-ad2e795b9f34",
            Title = "",
            Description = "",
            Price = 40,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 139,
            GUID = "2429736e-407b-4ad9-8521-0bcd075ff2d2",
            Title = "",
            Description = "",
            Price = 40,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 140,
            GUID = "b4979789-9955-4c39-b8ee-5778e5769310",
            Title = "",
            Description = "",
            Price = 10,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 141,
            GUID = "ea270102-7273-43db-884c-90dee0ba2a1c",
            Title = "",
            Description = "",
            Price = 10,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 142,
            GUID = "41f349ac-6ef9-47da-a8c8-1bd1800b3022",
            Title = "",
            Description = "",
            Price = 10,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 143,
            GUID = "9908842c-420d-4b44-830e-d8390ade5f10",
            Title = "",
            Description = "",
            Price = 10,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 144,
            GUID = "a610ea3c-1f2f-4459-8683-3e162693e8c5",
            Title = "",
            Description = "",
            Price = 10,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 145,
            GUID = "7e68f19f-3f9f-4965-86ed-414e24684a64",
            Title = "",
            Description = "",
            Price = 10,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 146,
            GUID = "20eac56f-16bf-40df-bb1a-2c76e6976ebd",
            Title = "",
            Description = "",
            Price = 10,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 147,
            GUID = "d45f4cfd-c6c7-4862-937d-6e3077ef7d59",
            Title = "",
            Description = "",
            Price = 10,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 148,
            GUID = "9598878f-8761-4d20-8728-d9b0ea213164",
            Title = "",
            Description = "",
            Price = 10,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 149,
            GUID = "5946aa88-e358-44ef-9239-0c4ff118231a",
            Title = "",
            Description = "",
            Price = 10,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 150,
            GUID = "90fabc7c-eff2-48ce-8ba5-bbbec15a6604",
            Title = "",
            Description = "",
            Price = 10,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 151,
            GUID = "f14a3cce-0e56-4306-b7e5-2d675510c5cd",
            Title = "",
            Description = "",
            Price = 10,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 152,
            GUID = "421bf7b5-7035-42f4-8e4f-9e6eaa078600",
            Title = "",
            Description = "",
            Price = 10,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 153,
            GUID = "2015e9f2-6ae3-4777-acbe-96500e87ab26",
            Title = "",
            Description = "",
            Price = 10,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 154,
            GUID = "aba09382-abba-4e08-bf2f-5e875895368e",
            Title = "",
            Description = "",
            Price = 10,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 155,
            GUID = "093c70e0-5f85-4fad-aa81-fb65fcfb672a",
            Title = "",
            Description = "",
            Price = 10,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 156,
            GUID = "eee82cfe-99a3-49a3-b902-5c5cfe29cf07",
            Title = "",
            Description = "",
            Price = 10,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 157,
            GUID = "59e85da2-872f-42a9-858e-3eab9f0d51fd",
            Title = "",
            Description = "",
            Price = 10,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 158,
            GUID = "5efaf3fe-3389-4574-a119-82e480c897d9",
            Title = "",
            Description = "",
            Price = 10,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 159,
            GUID = "c5323786-59e5-49f1-8dd6-03600e4cd6e6",
            Title = "",
            Description = "",
            Price = 10,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 160,
            GUID = "55b403a0-2fb1-456b-a356-ef57acd0e589",
            Title = "",
            Description = "",
            Price = 10,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 161,
            GUID = "36d51ff8-f438-497c-9ee5-546d6082b40e",
            Title = "",
            Description = "",
            Price = 10,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 162,
            GUID = "019dcaa6-98f6-48f6-b300-1da35967679c",
            Title = "",
            Description = "",
            Price = 10,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 163,
            GUID = "215d75b2-11c1-4432-82a7-1b3d522c04aa",
            Title = "",
            Description = "",
            Price = 10,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 164,
            GUID = "4df90626-4969-462b-8b95-d71dce5b73b4",
            Title = "",
            Description = "",
            Price = 10,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 165,
            GUID = "963bafba-c011-4166-85d6-230d10667525",
            Title = "",
            Description = "",
            Price = 10,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 166,
            GUID = "11700747-35d6-407e-bf8c-00aa5ec95ae2",
            Title = "",
            Description = "",
            Price = 10,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 167,
            GUID = "5c168acb-9437-4ebb-9df8-ca4076718b7b",
            Title = "",
            Description = "",
            Price = 10,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 168,
            GUID = "189c8f86-2dce-4ad8-ab9d-a9e9a4396f9d",
            Title = "",
            Description = "",
            Price = 999999999,
            VisibleInShop = false,
        },
        new ShopItemData
        {
            Id = 169,
            GUID = "4ca9ac8e-18dd-46e7-bb24-d44b8179e37e",
            Title = "",
            Description = "",
            Price = 999999999,
            VisibleInShop = false,
        },
        new ShopItemData
        {
            Id = 170,
            GUID = "e08fde2f-bc6e-4a25-b99c-54d53013d5bf",
            Title = "",
            Description = "",
            Price = 10,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 171,
            GUID = "79a18ff2-b128-4f8d-ad5e-d02dc12f54d5",
            Title = "",
            Description = "",
            Price = 10,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 172,
            GUID = "426a5ccc-c0fb-41f9-a77f-54f4c52bd983",
            Title = "",
            Description = "",
            Price = 10,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 173,
            GUID = "6ff05998-50dc-4a11-8729-5ce1465319c6",
            Title = "",
            Description = "",
            Price = 10,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 174,
            GUID = "5b1ba8f8-b06c-479e-8b8a-97770dc15c80",
            Title = "",
            Description = "",
            Price = 25,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 175,
            GUID = "76fc9177-3a1e-4af2-97fa-cafc9037aff7",
            Title = "",
            Description = "",
            Price = 25,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 176,
            GUID = "7e5bb04a-068b-4678-86b1-c0a1986a364d",
            Title = "",
            Description = "",
            Price = 25,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 177,
            GUID = "975d3913-3dab-4cc1-8962-97e26247fced",
            Title = "",
            Description = "",
            Price = 25,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 178,
            GUID = "6e09749e-bf98-4fb5-9080-b0fbeead45aa",
            Title = "",
            Description = "",
            Price = 25,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 179,
            GUID = "50b3dd18-54a0-47f7-b410-4a7822d11003",
            Title = "",
            Description = "",
            Price = 25,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 180,
            GUID = "7b553f10-87e7-47aa-aafe-83dc4c356391",
            Title = "",
            Description = "",
            Price = 25,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 181,
            GUID = "33b176ec-90be-4bf6-b457-c9f72122bb6f",
            Title = "",
            Description = "",
            Price = 25,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 182,
            GUID = "26936769-b058-4213-97bd-97a1b0b07b5b",
            Title = "",
            Description = "",
            Price = 25,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 183,
            GUID = "c640feee-1b24-4e64-9671-039198104ac9",
            Title = "",
            Description = "",
            Price = 25,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 184,
            GUID = "e1f1c808-86f6-4f46-95a6-732fdb362621",
            Title = "",
            Description = "",
            Price = 25,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 185,
            GUID = "86219a7b-0cca-4048-80dd-b9236a09cb76",
            Title = "",
            Description = "",
            Price = 25,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 186,
            GUID = "5625c879-c39a-4940-bfee-de535c553abb",
            Title = "",
            Description = "",
            Price = 40,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 187,
            GUID = "04f7659e-0ac4-4cca-ba2c-d47fa39b4114",
            Title = "",
            Description = "",
            Price = 40,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 188,
            GUID = "aab80a0f-84b1-4030-b731-e18ad351d55d",
            Title = "",
            Description = "",
            Price = 40,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 189,
            GUID = "88229d7b-5636-4a40-ab37-be3a21fb0709",
            Title = "",
            Description = "",
            Price = 40,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 190,
            GUID = "35354f99-817e-4780-9128-0c155539ca44",
            Title = "",
            Description = "",
            Price = 0,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 191,
            GUID = "4a8e8955-2ee7-48f0-8dbe-796ed0c61186",
            Title = "",
            Description = "",
            Price = 25,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 192,
            GUID = "c41d20b5-e909-4b4f-83fa-5a3896723abc",
            Title = "",
            Description = "",
            Price = 25,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 193,
            GUID = "5fc14ac2-2e3a-4e0c-93dc-4577986d122a",
            Title = "",
            Description = "",
            Price = 25,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 194,
            GUID = "1967af73-5337-4c22-af58-cbfa1f1b0618",
            Title = "",
            Description = "",
            Price = 25,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 195,
            GUID = "d37ebe2b-d6c5-4c73-a3a7-227099f0e1d9",
            Title = "",
            Description = "",
            Price = 25,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 196,
            GUID = "2d770d29-ef84-4215-8969-8158b5f0d3e8",
            Title = "",
            Description = "",
            Price = 25,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 197,
            GUID = "feb4c80e-a2a2-4aa4-a8e3-a8986480ab88",
            Title = "",
            Description = "",
            Price = 25,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 198,
            GUID = "9e94e264-0a0a-47ed-aa2b-7d23b780c984",
            Title = "",
            Description = "",
            Price = 10,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 199,
            GUID = "136c466a-3d45-4fd0-968e-47fd9ce4c7f2",
            Title = "",
            Description = "",
            Price = 10,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 200,
            GUID = "10f47a91-1416-4638-9530-92227fd830e0",
            Title = "",
            Description = "",
            Price = 10,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 201,
            GUID = "2954e6c5-7cef-4759-aa19-6ead8c78679f",
            Title = "",
            Description = "",
            Price = 10,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 202,
            GUID = "a187dbd3-c3f2-4b5e-8846-cc96c4b8434a",
            Title = "",
            Description = "",
            Price = 50,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 203,
            GUID = "cdcfca92-7c71-4369-b054-12328725a171",
            Title = "",
            Description = "",
            Price = 50,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 204,
            GUID = "00de3a45-c666-425d-92c3-0b71fc961eb5",
            Title = "",
            Description = "",
            Price = 50,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 205,
            GUID = "5dfbf9d2-6449-4ecb-824b-ba2902f14002",
            Title = "",
            Description = "",
            Price = 50,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 206,
            GUID = "5233e20f-93a2-46c5-bf34-07de6d3e77f6",
            Title = "",
            Description = "",
            Price = 50,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 207,
            GUID = "0497cce3-0a1a-447e-9cb4-2e8634147e9f",
            Title = "",
            Description = "",
            Price = 50,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 208,
            GUID = "9d7fba8a-7c5a-41dd-8ea7-3dfa21f0f627",
            Title = "",
            Description = "",
            Price = 50,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 209,
            GUID = "4947974c-988a-44eb-9434-6097af20779d",
            Title = "",
            Description = "",
            Price = 50,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 210,
            GUID = "2baeb1c6-bd8d-4060-81a9-4a88007cb11e",
            Title = "",
            Description = "",
            Price = 50,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 211,
            GUID = "3891782f-59f9-4558-8094-cb87eddb1ce7",
            Title = "",
            Description = "",
            Price = 50,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 212,
            GUID = "50dd779f-6715-4f7a-9f6f-0979d39d5a9a",
            Title = "",
            Description = "",
            Price = 50,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 213,
            GUID = "bf258382-0f2e-46e7-a466-e71e0386f9b1",
            Title = "",
            Description = "",
            Price = 50,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 214,
            GUID = "cb3d4056-f5bb-4ffa-8e72-3f9342906b33",
            Title = "",
            Description = "",
            Price = 0,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 215,
            GUID = "27951e71-3f67-405d-a504-f755187b4bb6",
            Title = "",
            Description = "",
            Price = 20,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 216,
            GUID = "01b04823-246a-48ca-9ded-a923280c9c1a",
            Title = "",
            Description = "",
            Price = 20,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 217,
            GUID = "ae6b64d0-495e-4946-85df-2d2cbcea4aab",
            Title = "",
            Description = "",
            Price = 20,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 218,
            GUID = "aefb40a6-97b5-401e-8d41-406bd158b2d2",
            Title = "",
            Description = "",
            Price = 30,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 219,
            GUID = "c9410122-2699-4c88-aee7-a4d8c6dd16e1",
            Title = "",
            Description = "",
            Price = 30,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 220,
            GUID = "9fe688a6-3218-4d0c-bb5b-237db42b3d12",
            Title = "",
            Description = "",
            Price = 30,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 221,
            GUID = "1a3b8c5c-c864-452f-b02c-1aa4a38158f0",
            Title = "",
            Description = "",
            Price = 30,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 222,
            GUID = "11ae2951-4922-49af-b876-5c1a6165b866",
            Title = "",
            Description = "",
            Price = 30,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 223,
            GUID = "04c38ef6-2490-434f-8077-130ba42e6108",
            Title = "",
            Description = "",
            Price = 30,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 224,
            GUID = "969243fc-76c7-453b-824e-65a1fd069866",
            Title = "",
            Description = "",
            Price = 30,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 225,
            GUID = "15100015-af18-4c33-82aa-3cd05409d4b6",
            Title = "",
            Description = "",
            Price = 30,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 226,
            GUID = "b2d15bfd-805d-43a9-84f5-00bfe6c09f9a",
            Title = "",
            Description = "",
            Price = 30,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 227,
            GUID = "bc43d149-fcae-4831-a721-f0de7e1a82e9",
            Title = "",
            Description = "",
            Price = 30,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 228,
            GUID = "c23ffc17-8655-49ef-ba0c-ed644c5a85d8",
            Title = "",
            Description = "",
            Price = 30,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 229,
            GUID = "9f9a4300-5f2e-4fd8-907c-6aa30dda7123",
            Title = "",
            Description = "",
            Price = 30,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 230,
            GUID = "2baf1e5c-9391-4884-b26f-43b0c2e7ab42",
            Title = "",
            Description = "",
            Price = 30,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 231,
            GUID = "90e4812e-4cdc-4be2-8d6f-12296b43821c",
            Title = "",
            Description = "",
            Price = 30,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 232,
            GUID = "99108e53-c2d2-4b93-a90a-d1b5bf9bbda6",
            Title = "",
            Description = "",
            Price = 30,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 233,
            GUID = "cee1dc26-a7cf-4236-af76-dcc8e58f2634",
            Title = "",
            Description = "",
            Price = 30,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 234,
            GUID = "9116b396-6110-470c-be13-0014d304f87e",
            Title = "",
            Description = "",
            Price = 40,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 235,
            GUID = "04aa3e31-e45d-44da-9fe5-22fa61a044fe",
            Title = "",
            Description = "",
            Price = 40,
            VisibleInShop = true,
        }
    };

    public static ShopItemData[] Boy_Cat_Items = new ShopItemData[]
    {
        new ShopItemData
        {
            Id = 236,
            GUID = "d2ff4d60-c06a-498d-bac0-df2578334b80",
            Title = "",
            Description = "",
            Price = 0,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 237,
            GUID = "8d054591-1f43-45c8-867d-526a57e4e6d9",
            Title = "",
            Description = "",
            Price = 0,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 238,
            GUID = "a2cce0f5-632b-4134-9599-18ef434a4a69",
            Title = "",
            Description = "",
            Price = 0,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 239,
            GUID = "6d283eba-ef47-4ca5-8145-7493e05e411c",
            Title = "",
            Description = "",
            Price = 0,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 240,
            GUID = "8e0fd6c6-e5ba-49db-8372-8236764d87b8",
            Title = "",
            Description = "",
            Price = 0,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 241,
            GUID = "2d45f8e0-eb93-4a38-b934-ae56609917aa",
            Title = "",
            Description = "",
            Price = 0,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 242,
            GUID = "53887289-1ad4-4ca2-ba0f-d9dc814d981f",
            Title = "",
            Description = "",
            Price = 0,
            VisibleInShop = false,
        },
        new ShopItemData
        {
            Id = 243,
            GUID = "43e4d57f-0fb3-465e-8dca-67b2dc8bb03b",
            Title = "",
            Description = "",
            Price = 0,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 244,
            GUID = "a62640c4-1e2f-46a6-a465-f2707e6bbd92",
            Title = "",
            Description = "",
            Price = 0,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 245,
            GUID = "3c834ebf-e6cd-4aaf-aafa-3266d02813b6",
            Title = "",
            Description = "",
            Price = 40,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 246,
            GUID = "698d53cf-e204-423a-a5cf-ec0070fa9938",
            Title = "",
            Description = "",
            Price = 40,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 247,
            GUID = "235296f2-21ef-47e7-a970-85d49004924d",
            Title = "",
            Description = "",
            Price = 40,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 248,
            GUID = "d23aa275-3807-4af0-b897-a6b82c2d55b8",
            Title = "",
            Description = "",
            Price = 25,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 249,
            GUID = "59e6ea87-e7cc-4213-8321-337e9ee1dd39",
            Title = "",
            Description = "",
            Price = 25,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 250,
            GUID = "16ffbea1-fb38-43b7-9a1e-fbb9fccc53e1",
            Title = "",
            Description = "",
            Price = 25,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 251,
            GUID = "95dcdf04-1f12-459e-93b5-fa5fe3a46208",
            Title = "",
            Description = "",
            Price = 25,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 252,
            GUID = "6506fb94-7248-4b18-8f38-3290f9c68ec3",
            Title = "",
            Description = "",
            Price = 40,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 253,
            GUID = "0be543ed-8c95-427a-bf4b-be7c31b9e5f8",
            Title = "",
            Description = "",
            Price = 40,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 254,
            GUID = "ac8fad15-eb00-4dfb-9c00-257b4fc911a8",
            Title = "",
            Description = "",
            Price = 40,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 255,
            GUID = "dc0978b6-b65a-49ff-b84b-3a8a972c0a9f",
            Title = "",
            Description = "",
            Price = 40,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 256,
            GUID = "d1dc595b-ccc2-4c49-85f4-32f632de2348",
            Title = "",
            Description = "",
            Price = 10,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 257,
            GUID = "624e2331-e576-4e21-bfd0-eb0d3e4cedac",
            Title = "",
            Description = "",
            Price = 10,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 258,
            GUID = "77f64711-d2bc-43c7-bc42-42b1a568889a",
            Title = "",
            Description = "",
            Price = 10,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 259,
            GUID = "9370920b-84a6-4770-a8fc-14975fd392fe",
            Title = "",
            Description = "",
            Price = 10,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 260,
            GUID = "c43dd92f-3eaf-41af-b0c0-e8b097498644",
            Title = "",
            Description = "",
            Price = 10,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 261,
            GUID = "275fa9ef-4fc7-4029-9e7e-e8a2d8e7831f",
            Title = "",
            Description = "",
            Price = 10,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 262,
            GUID = "8470e9a2-5063-4171-83cb-db0e0cdfec49",
            Title = "",
            Description = "",
            Price = 10,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 263,
            GUID = "b22f7f29-e360-4c94-8a0b-96742610bf0f",
            Title = "",
            Description = "",
            Price = 10,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 264,
            GUID = "3eeb04eb-37ba-4a5d-9724-3ea9224f4bc8",
            Title = "",
            Description = "",
            Price = 10,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 265,
            GUID = "f62fe871-ddc4-4493-89f0-436454261c81",
            Title = "",
            Description = "",
            Price = 10,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 266,
            GUID = "8fb70f4c-235c-4730-bdd5-a3bb71689f15",
            Title = "",
            Description = "",
            Price = 10,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 267,
            GUID = "f2101eec-fc68-4ca8-b35c-6de5031acaa6",
            Title = "",
            Description = "",
            Price = 10,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 268,
            GUID = "416c28e6-c495-4afb-9da0-b51c9b616d91",
            Title = "",
            Description = "",
            Price = 10,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 269,
            GUID = "31558ec7-b3ea-43cc-ac20-b6281fddf7d8",
            Title = "",
            Description = "",
            Price = 10,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 270,
            GUID = "4421f13f-d2ce-45b6-8ccd-76bbfdd9a14b",
            Title = "",
            Description = "",
            Price = 10,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 271,
            GUID = "f1047ace-2a17-45e8-8fa4-ef54b04b738b",
            Title = "",
            Description = "",
            Price = 10,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 272,
            GUID = "7474f63b-347f-4888-a400-fcbbfc75ed0b",
            Title = "",
            Description = "",
            Price = 10,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 273,
            GUID = "89515818-8372-4812-bfaf-f96e1b61858f",
            Title = "",
            Description = "",
            Price = 10,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 274,
            GUID = "49ef6968-d093-4ebc-a8f6-e3db48dae357",
            Title = "",
            Description = "",
            Price = 10,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 275,
            GUID = "6fd048b0-51a6-4fc3-b832-c6e9abc26640",
            Title = "",
            Description = "",
            Price = 10,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 276,
            GUID = "49bff5db-f8b1-47bc-9197-b1c2df7f5bcb",
            Title = "",
            Description = "",
            Price = 10,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 277,
            GUID = "19bf1e13-3aea-45d6-ba54-e53388e555e9",
            Title = "",
            Description = "",
            Price = 10,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 278,
            GUID = "25d30805-df06-4c4f-8192-79fb92e63d69",
            Title = "",
            Description = "",
            Price = 10,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 279,
            GUID = "e764220e-4501-4c17-a354-b01c48cca3d4",
            Title = "",
            Description = "",
            Price = 10,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 280,
            GUID = "97de8e4c-a483-4207-94ee-e152bdd23da1",
            Title = "",
            Description = "",
            Price = 10,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 281,
            GUID = "e77677e5-8788-4a77-ab14-c5b52e555137",
            Title = "",
            Description = "",
            Price = 10,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 282,
            GUID = "faad601c-1631-4447-abb7-08b4a37421b0",
            Title = "",
            Description = "",
            Price = 10,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 283,
            GUID = "f809fae8-f3d0-4160-a4d5-0900d3523ace",
            Title = "",
            Description = "",
            Price = 10,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 284,
            GUID = "0ffd297c-a9d9-40b6-bdcf-27cb3353daa5",
            Title = "",
            Description = "",
            Price = 999999999,
            VisibleInShop = false,
        },
        new ShopItemData
        {
            Id = 285,
            GUID = "30f854b0-d554-4044-85c5-b8cd66667383",
            Title = "",
            Description = "",
            Price = 999999999,
            VisibleInShop = false,
        },
        new ShopItemData
        {
            Id = 286,
            GUID = "0bdf2a34-6e92-4eb5-9e5a-9f8fe831b3ab",
            Title = "",
            Description = "",
            Price = 10,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 287,
            GUID = "8adc367b-18e7-47e4-89ad-20d752d42596",
            Title = "",
            Description = "",
            Price = 10,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 288,
            GUID = "d2474756-8342-4c25-bacf-d96f3e259f47",
            Title = "",
            Description = "",
            Price = 10,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 289,
            GUID = "99fba25c-22bc-41b4-96c0-d7f76d3ca2e1",
            Title = "",
            Description = "",
            Price = 10,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 290,
            GUID = "899daf02-2ea9-4d66-8393-42cc86c45e23",
            Title = "",
            Description = "",
            Price = 25,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 291,
            GUID = "dd20d4ce-3ebb-4f51-aae6-b9ffcdb0c3b3",
            Title = "",
            Description = "",
            Price = 25,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 292,
            GUID = "70ddeb31-9fee-4fb9-856b-b9c2329ae9af",
            Title = "",
            Description = "",
            Price = 25,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 293,
            GUID = "1af8b5bc-24d7-4a05-8e24-6ccb1d4b75f1",
            Title = "",
            Description = "",
            Price = 25,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 294,
            GUID = "b1291e69-6ec5-4cf3-af88-632aa32295e8",
            Title = "",
            Description = "",
            Price = 25,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 295,
            GUID = "cf89ced8-e549-485f-9ea7-69a7807e5d0c",
            Title = "",
            Description = "",
            Price = 25,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 296,
            GUID = "8afd9fa0-27ed-4a3c-9e62-a26f3b00ab7f",
            Title = "",
            Description = "",
            Price = 25,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 297,
            GUID = "38cbafcb-6cee-4b50-8e34-9951c57eb7dc",
            Title = "",
            Description = "",
            Price = 25,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 298,
            GUID = "b37b92e1-6551-4dda-b2a7-9c1b5afeb84e",
            Title = "",
            Description = "",
            Price = 25,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 299,
            GUID = "80226705-b061-4a00-a5f7-2269aacffe3d",
            Title = "",
            Description = "",
            Price = 25,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 300,
            GUID = "518a4f61-f170-4713-9aa5-d2a4bba60a2b",
            Title = "",
            Description = "",
            Price = 25,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 301,
            GUID = "a0fc57e0-e845-4de9-8c2e-d1f36a4cefd9",
            Title = "",
            Description = "",
            Price = 25,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 302,
            GUID = "0e9c43d3-5b87-4ddd-9dab-080352aeef51",
            Title = "",
            Description = "",
            Price = 40,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 303,
            GUID = "d86b1bf3-a862-4d74-9c4e-ba2c67da05e1",
            Title = "",
            Description = "",
            Price = 40,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 304,
            GUID = "d312a187-9d13-45fd-8cd5-e9ef4ab6e88f",
            Title = "",
            Description = "",
            Price = 40,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 305,
            GUID = "25e67c05-ed73-45cf-b44b-0bdac8374f7b",
            Title = "",
            Description = "",
            Price = 40,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 306,
            GUID = "6bd1489a-5c8d-48b1-be31-044c6d5fc3da",
            Title = "",
            Description = "",
            Price = 0,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 307,
            GUID = "5ca1bc75-3b6a-4135-bca3-313f9bee9874",
            Title = "",
            Description = "",
            Price = 25,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 308,
            GUID = "4fd5db9d-aeb6-4edf-97c7-4ca065492e59",
            Title = "",
            Description = "",
            Price = 25,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 309,
            GUID = "95e6c08c-d990-4bed-8df4-b44d0b0781de",
            Title = "",
            Description = "",
            Price = 25,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 310,
            GUID = "8712df12-5e08-4296-ab80-d0b7cdb62c1b",
            Title = "",
            Description = "",
            Price = 25,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 311,
            GUID = "fbd113c1-856d-4f80-a15c-22e73fad0b21",
            Title = "",
            Description = "",
            Price = 25,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 312,
            GUID = "248934f1-35ec-4eff-b5e7-c8dcc140a950",
            Title = "",
            Description = "",
            Price = 25,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 313,
            GUID = "267e8620-10a2-4be5-8a6a-cde063699ed0",
            Title = "",
            Description = "",
            Price = 25,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 314,
            GUID = "3cff196d-8e87-4a7f-92d8-0bbed41bb099",
            Title = "",
            Description = "",
            Price = 10,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 315,
            GUID = "57a7bb33-b72a-44f8-8e25-29cfffd7c1c7",
            Title = "",
            Description = "",
            Price = 10,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 316,
            GUID = "3e10b224-3216-497e-800e-85a648449908",
            Title = "",
            Description = "",
            Price = 10,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 317,
            GUID = "14e9f837-7bc8-4619-a181-391dd0f5aa6f",
            Title = "",
            Description = "",
            Price = 10,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 318,
            GUID = "d0ccd4f2-2bb3-4714-8777-adaf76c9644c",
            Title = "",
            Description = "",
            Price = 50,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 319,
            GUID = "17938db6-a855-465b-8bb6-4b8d30e14f3e",
            Title = "",
            Description = "",
            Price = 50,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 320,
            GUID = "4579abb8-13df-4bef-96f8-e73584776b7f",
            Title = "",
            Description = "",
            Price = 50,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 321,
            GUID = "a8865f8a-c1b7-4ee3-9d04-a31dc0e7b8c2",
            Title = "",
            Description = "",
            Price = 50,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 322,
            GUID = "57bac90f-3b64-44eb-8eea-287bb962ff3e",
            Title = "",
            Description = "",
            Price = 50,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 323,
            GUID = "3b517d2f-a576-45c8-a1d8-b2de99ffd4c6",
            Title = "",
            Description = "",
            Price = 50,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 324,
            GUID = "ff2518ff-8c19-453e-abad-781fdb69d37c",
            Title = "",
            Description = "",
            Price = 50,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 325,
            GUID = "a52fa412-7ea4-4757-a55a-4fc46aa19c72",
            Title = "",
            Description = "",
            Price = 50,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 326,
            GUID = "f6fb45d6-f52a-4cb9-9bcd-eb00e1eaa196",
            Title = "",
            Description = "",
            Price = 50,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 327,
            GUID = "ed2b316a-0bd8-491e-89cf-ede8fcc4b573",
            Title = "",
            Description = "",
            Price = 50,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 328,
            GUID = "53238297-a92c-4ac2-952d-57cfca0af336",
            Title = "",
            Description = "",
            Price = 50,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 329,
            GUID = "6c4a6eba-a71b-4912-82c2-2fcbd0160640",
            Title = "",
            Description = "",
            Price = 50,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 330,
            GUID = "e9c946c0-465c-428c-b67d-2c4c037f7fe2",
            Title = "",
            Description = "",
            Price = 0,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 331,
            GUID = "5f63c718-5895-4138-8510-0d01c4a3c143",
            Title = "",
            Description = "",
            Price = 20,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 332,
            GUID = "92464671-0a2c-42d9-97ad-14afefa7fc46",
            Title = "",
            Description = "",
            Price = 20,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 333,
            GUID = "97d203d8-1d6c-4dc8-aa92-87ec88aad5bc",
            Title = "",
            Description = "",
            Price = 20,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 334,
            GUID = "de309d2e-5229-4729-a3cc-1b608c452be9",
            Title = "",
            Description = "",
            Price = 30,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 335,
            GUID = "892dedb7-440e-47ae-a8e4-0abab0e32f11",
            Title = "",
            Description = "",
            Price = 30,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 336,
            GUID = "0a98a50d-1237-4cb5-8f33-0dc75940717a",
            Title = "",
            Description = "",
            Price = 30,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 337,
            GUID = "10bfe0cb-014a-434b-bc71-afd5627e2452",
            Title = "",
            Description = "",
            Price = 30,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 338,
            GUID = "fc9063c0-7296-4f3d-8106-26b901f50b03",
            Title = "",
            Description = "",
            Price = 30,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 339,
            GUID = "e18b435f-0843-4d76-89ea-10339d076b31",
            Title = "",
            Description = "",
            Price = 30,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 340,
            GUID = "6414fea9-6dd4-4269-a82d-d2598b4fcf27",
            Title = "",
            Description = "",
            Price = 30,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 341,
            GUID = "f5731811-ceb4-4fc2-993e-d4d2218d3bed",
            Title = "",
            Description = "",
            Price = 30,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 342,
            GUID = "1424c041-a32d-407f-b4e9-9c4c2ea0e93f",
            Title = "",
            Description = "",
            Price = 30,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 343,
            GUID = "794f891b-3d4f-4b2d-92ed-f9e3111af8fd",
            Title = "",
            Description = "",
            Price = 30,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 344,
            GUID = "35ad0c1c-ddd1-4961-a277-5cf90ada6af4",
            Title = "",
            Description = "",
            Price = 30,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 345,
            GUID = "5849675c-4359-4c48-8fc0-3bb2144e800f",
            Title = "",
            Description = "",
            Price = 30,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 346,
            GUID = "983b2b74-d5d9-4ca5-9835-1391157d12e2",
            Title = "",
            Description = "",
            Price = 30,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 347,
            GUID = "dc97a11f-0272-4980-b35b-37c089d6598b",
            Title = "",
            Description = "",
            Price = 30,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 348,
            GUID = "c031f35d-985c-42d5-827b-36cba2ceb2f5",
            Title = "",
            Description = "",
            Price = 30,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 349,
            GUID = "6e49d6b2-2912-46df-96a5-9eb19afdabd6",
            Title = "",
            Description = "",
            Price = 30,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 350,
            GUID = "6a52fb9b-1166-4fcf-beaf-c07985263122",
            Title = "",
            Description = "",
            Price = 40,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 351,
            GUID = "b7f3f978-8cb7-4a39-afd8-db7087a79230",
            Title = "",
            Description = "",
            Price = 40,
            VisibleInShop = true,
        }
    };

    public static ShopItemData[] Girl_Dog_Items = new ShopItemData[]
    {
        new ShopItemData
        {
            Id = 352,
            GUID = "b6b5e488-bd5b-496f-8318-85b1a24086ae",
            Title = "",
            Description = "",
            Price = 0,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 353,
            GUID = "d2d0592f-16bd-45bd-bd9f-5cd01549db30",
            Title = "",
            Description = "",
            Price = 0,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 354,
            GUID = "7fd66457-6f9e-4b29-aa8e-8d7005bd1169",
            Title = "",
            Description = "",
            Price = 0,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 355,
            GUID = "ea7778c4-62f1-4bd7-b35c-7c4f7e7d4c0d",
            Title = "",
            Description = "",
            Price = 0,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 356,
            GUID = "4e8e84d0-f293-4002-81b0-1108d9efa0bd",
            Title = "",
            Description = "",
            Price = 0,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 357,
            GUID = "2d5bdb40-5e15-481d-bef1-ed89e2b823ed",
            Title = "",
            Description = "",
            Price = 0,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 358,
            GUID = "26712d86-cb44-4c17-bc4e-21f73d91682d",
            Title = "",
            Description = "",
            Price = 0,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 359,
            GUID = "b61ed0a5-bd0c-4351-9405-d6b6edf57ce8",
            Title = "",
            Description = "",
            Price = 0,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 360,
            GUID = "c98c63cd-55f6-4593-9edf-1e6a43b9ba14",
            Title = "",
            Description = "",
            Price = 0,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 361,
            GUID = "d9346be4-673d-489e-a1dd-273086db1a9a",
            Title = "",
            Description = "",
            Price = 40,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 362,
            GUID = "ab695742-f59c-491e-8f93-beea7cf59f81",
            Title = "",
            Description = "",
            Price = 40,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 363,
            GUID = "9266dafd-4458-4a08-b1b5-9c026f0e7732",
            Title = "",
            Description = "",
            Price = 40,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 364,
            GUID = "4fd50da6-9323-4af0-8d59-fdff27aab8a6",
            Title = "",
            Description = "",
            Price = 25,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 365,
            GUID = "4138d8de-3074-41fa-82c4-b38b2c18f59d",
            Title = "",
            Description = "",
            Price = 25,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 366,
            GUID = "21709115-a2a4-4d0a-ad60-d98f812f7471",
            Title = "",
            Description = "",
            Price = 25,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 367,
            GUID = "a8e36c4f-3b65-43b1-9ff3-c69bf1049b97",
            Title = "",
            Description = "",
            Price = 25,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 368,
            GUID = "353f4be4-9037-4e19-ad71-b3a05f5961d9",
            Title = "",
            Description = "",
            Price = 40,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 369,
            GUID = "9649d418-58e8-49ec-a8b3-68f7204fe467",
            Title = "",
            Description = "",
            Price = 40,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 370,
            GUID = "dac05f9e-e4be-400e-ba50-28d49bb181f6",
            Title = "",
            Description = "",
            Price = 40,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 371,
            GUID = "d2fa287d-fc77-48dd-8ae9-2302ae7d9906",
            Title = "",
            Description = "",
            Price = 40,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 372,
            GUID = "75ce58e6-9b6f-47c4-9b7a-7bd9aa42631a",
            Title = "",
            Description = "",
            Price = 10,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 373,
            GUID = "e73e1f89-b605-418c-b87e-63ed78acfb24",
            Title = "",
            Description = "",
            Price = 10,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 374,
            GUID = "9b924103-89b9-434d-aeb4-36cb57bab981",
            Title = "",
            Description = "",
            Price = 10,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 375,
            GUID = "97d137ac-5839-4299-a49f-6e7156d2edf2",
            Title = "",
            Description = "",
            Price = 10,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 376,
            GUID = "6b362573-d0bd-42be-8cfa-195ab8a86a5a",
            Title = "",
            Description = "",
            Price = 10,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 377,
            GUID = "0be60a12-3e04-47ef-bc5f-37efc3ba271d",
            Title = "",
            Description = "",
            Price = 10,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 378,
            GUID = "a756a526-d21a-4087-ba92-d893f1d7b27b",
            Title = "",
            Description = "",
            Price = 10,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 379,
            GUID = "cef657c4-5f12-42c3-9597-de1d750946c7",
            Title = "",
            Description = "",
            Price = 10,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 380,
            GUID = "cf60bc3e-003a-4abb-af92-ea492e3ae987",
            Title = "",
            Description = "",
            Price = 10,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 381,
            GUID = "f4e7e8c1-ae41-4a6c-b65b-b64afb93ab67",
            Title = "",
            Description = "",
            Price = 10,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 382,
            GUID = "7070e929-8918-439d-be6b-053a3efbec1d",
            Title = "",
            Description = "",
            Price = 10,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 383,
            GUID = "03c4f8fb-4585-44a5-8fb0-e28738d037ae",
            Title = "",
            Description = "",
            Price = 10,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 384,
            GUID = "6cea091d-1fba-43a5-9cec-e016fef6b11f",
            Title = "",
            Description = "",
            Price = 10,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 385,
            GUID = "f19c02a2-3b64-44ca-aaee-8292dc47bac6",
            Title = "",
            Description = "",
            Price = 10,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 386,
            GUID = "287d2597-2d98-4bfd-980c-af59d91f668b",
            Title = "",
            Description = "",
            Price = 10,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 387,
            GUID = "fdfbb640-6120-495e-b518-f734d10f241e",
            Title = "",
            Description = "",
            Price = 10,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 388,
            GUID = "b9ecab0c-319f-45de-9b46-f0a3b3f8068c",
            Title = "",
            Description = "",
            Price = 10,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 389,
            GUID = "58f6c497-7d9d-4980-8679-a64d46f5eec7",
            Title = "",
            Description = "",
            Price = 10,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 390,
            GUID = "5eb52a92-4e03-4562-82ae-f31464ac32a4",
            Title = "",
            Description = "",
            Price = 10,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 391,
            GUID = "13f322aa-83d8-410e-9cfa-3425948c1aa8",
            Title = "",
            Description = "",
            Price = 10,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 392,
            GUID = "3734e05e-c5a1-4c98-a72c-54c4e420dd49",
            Title = "",
            Description = "",
            Price = 10,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 393,
            GUID = "74e3f165-b5ac-417a-a25e-2799bf8785fd",
            Title = "",
            Description = "",
            Price = 10,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 394,
            GUID = "a7879c7c-dd78-4831-af74-9d8ce2115e4e",
            Title = "",
            Description = "",
            Price = 10,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 395,
            GUID = "e0dd3446-37f3-4d64-8aba-6f173bfe4290",
            Title = "",
            Description = "",
            Price = 10,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 396,
            GUID = "d4620888-7ecf-41c1-93f7-acfea08c614a",
            Title = "",
            Description = "",
            Price = 10,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 397,
            GUID = "89e9dc57-da1b-469d-bf01-3881bb089fa1",
            Title = "",
            Description = "",
            Price = 10,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 398,
            GUID = "e35726b9-8df4-45d9-a94a-0f28117bf638",
            Title = "",
            Description = "",
            Price = 10,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 399,
            GUID = "1401d264-d3e5-4417-b1dc-38fe552dd784",
            Title = "",
            Description = "",
            Price = 10,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 400,
            GUID = "c60a6791-70f3-47bb-ba70-0667e5139628",
            Title = "",
            Description = "",
            Price = 999999999,
            VisibleInShop = false,
        },
        new ShopItemData
        {
            Id = 401,
            GUID = "cb81f4c4-558f-45ab-b3a6-e22391481d41",
            Title = "",
            Description = "",
            Price = 999999999,
            VisibleInShop = false,
        },
        new ShopItemData
        {
            Id = 402,
            GUID = "6afbe627-0525-4418-9ab8-a6d22107d726",
            Title = "",
            Description = "",
            Price = 10,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 403,
            GUID = "d0b39704-1143-4e29-9197-4d3faa576e80",
            Title = "",
            Description = "",
            Price = 10,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 404,
            GUID = "db77d511-c13c-4f45-9eeb-85f530f08bc3",
            Title = "",
            Description = "",
            Price = 10,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 405,
            GUID = "469817fd-0675-4e38-9d95-98a3d047fe77",
            Title = "",
            Description = "",
            Price = 10,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 406,
            GUID = "0f9660b8-4280-45f2-8e7d-cd8b93efaa44",
            Title = "",
            Description = "",
            Price = 25,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 407,
            GUID = "38a4c996-bd93-49d9-88a5-bd3f6c3b68f8",
            Title = "",
            Description = "",
            Price = 25,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 408,
            GUID = "75094b9e-8f2b-4353-b576-39bd4d69259b",
            Title = "",
            Description = "",
            Price = 25,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 409,
            GUID = "cc6aea15-8bfc-47c0-9b43-efeaa7dd29a2",
            Title = "",
            Description = "",
            Price = 25,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 410,
            GUID = "7abad326-0095-47c2-a1c6-97f6315a418d",
            Title = "",
            Description = "",
            Price = 25,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 411,
            GUID = "9979c3ab-f1e2-4c87-9aea-959c2d22099d",
            Title = "",
            Description = "",
            Price = 25,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 412,
            GUID = "3b4ff95c-4833-4ada-96dc-a60a8d1230ac",
            Title = "",
            Description = "",
            Price = 25,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 413,
            GUID = "7aab61ad-fd64-4286-89f1-c90a414cc0f6",
            Title = "",
            Description = "",
            Price = 25,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 414,
            GUID = "c0c8c695-6725-4a42-9e83-e3d93869afb6",
            Title = "",
            Description = "",
            Price = 25,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 415,
            GUID = "473da55a-70de-4a7d-8dde-e44ba0ef5964",
            Title = "",
            Description = "",
            Price = 25,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 416,
            GUID = "fc4336f7-9eeb-4a14-8a63-38eee0912d59",
            Title = "",
            Description = "",
            Price = 25,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 417,
            GUID = "c3026339-fabe-4c99-bdba-f0240ab6ffdb",
            Title = "",
            Description = "",
            Price = 25,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 418,
            GUID = "7f55eab3-75ac-49a3-a29b-860599263272",
            Title = "",
            Description = "",
            Price = 40,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 419,
            GUID = "e617501b-5bef-4616-9845-e357fe1d0782",
            Title = "",
            Description = "",
            Price = 40,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 420,
            GUID = "135b2384-19e6-45ec-97e5-e0b776b4115d",
            Title = "",
            Description = "",
            Price = 40,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 421,
            GUID = "187f9b49-f6cd-4644-b6a2-ad9fbf58a9d2",
            Title = "",
            Description = "",
            Price = 40,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 422,
            GUID = "ecab9602-d5c9-4a0c-901b-9cb7fd0e353f",
            Title = "",
            Description = "",
            Price = 0,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 423,
            GUID = "cc1b9a28-5ddc-44e9-9ec4-9dd4640b3627",
            Title = "",
            Description = "",
            Price = 25,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 424,
            GUID = "a52ef46b-3369-48d3-a2b9-d8b15b2821d2",
            Title = "",
            Description = "",
            Price = 25,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 425,
            GUID = "253f5941-2866-45b5-a8a0-787701198284",
            Title = "",
            Description = "",
            Price = 25,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 426,
            GUID = "8e65308c-1642-4abf-a55b-7b6e5e877f50",
            Title = "",
            Description = "",
            Price = 25,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 427,
            GUID = "5c9762f3-8970-42d4-a8e7-e07f375ef3a3",
            Title = "",
            Description = "",
            Price = 25,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 428,
            GUID = "b0564bb9-9375-476b-ba3e-fb12f17d1660",
            Title = "",
            Description = "",
            Price = 25,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 429,
            GUID = "3c46f806-6af0-4e8f-9a39-0cbcff74b1b0",
            Title = "",
            Description = "",
            Price = 25,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 430,
            GUID = "39bd0610-3f8c-42f5-9c66-5735206fdc4c",
            Title = "",
            Description = "",
            Price = 10,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 431,
            GUID = "f6f9c7e0-addf-40ef-8970-30e41db7a2c3",
            Title = "",
            Description = "",
            Price = 10,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 432,
            GUID = "dfd8a9ae-125d-4985-b106-1ad0f82793b5",
            Title = "",
            Description = "",
            Price = 10,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 433,
            GUID = "f113f999-018b-40e6-81f3-11b2d1ebabf5",
            Title = "",
            Description = "",
            Price = 10,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 434,
            GUID = "601e98ed-d0f8-46bf-bff2-4f6d1b83f274",
            Title = "",
            Description = "",
            Price = 50,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 435,
            GUID = "c1b10b6a-a68f-48bc-8b2c-ebe98e64bd19",
            Title = "",
            Description = "",
            Price = 50,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 436,
            GUID = "2cf5a0b7-bf6f-467b-b5e3-cd0905e84dc2",
            Title = "",
            Description = "",
            Price = 50,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 437,
            GUID = "3f89377a-068b-4375-89df-16218a16fd6a",
            Title = "",
            Description = "",
            Price = 50,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 438,
            GUID = "e0a9be88-3370-427d-b7ce-0eb29c5f7719",
            Title = "",
            Description = "",
            Price = 50,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 439,
            GUID = "a7b03419-379f-414d-aa40-965d6308fdf2",
            Title = "",
            Description = "",
            Price = 50,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 440,
            GUID = "406e5174-5605-480a-b9b4-5a630541b7d1",
            Title = "",
            Description = "",
            Price = 50,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 441,
            GUID = "8b5f6fb1-ca35-4092-95bb-5e0c442af3f9",
            Title = "",
            Description = "",
            Price = 50,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 442,
            GUID = "8b0b3bfb-d120-45b3-a694-7495af3692f6",
            Title = "",
            Description = "",
            Price = 50,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 443,
            GUID = "bc2a6b6e-a802-46a6-b0b7-e4e327f61a24",
            Title = "",
            Description = "",
            Price = 50,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 444,
            GUID = "62129a8c-6538-479a-94e2-ea79d60f04c3",
            Title = "",
            Description = "",
            Price = 50,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 445,
            GUID = "b48d24f0-aaae-49e6-80aa-212fe95e7677",
            Title = "",
            Description = "",
            Price = 50,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 446,
            GUID = "77908a33-1286-4148-8b41-855acd0feb3c",
            Title = "",
            Description = "",
            Price = 50,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 447,
            GUID = "2d1c8dd4-d1e0-41c9-86d5-fe990bfd35f3",
            Title = "",
            Description = "",
            Price = 50,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 448,
            GUID = "0a6429eb-9efc-45f4-839c-7fc0a9ec3d30",
            Title = "",
            Description = "",
            Price = 50,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 449,
            GUID = "f1646000-246d-4df0-ab62-0ccc3456076f",
            Title = "",
            Description = "",
            Price = 50,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 450,
            GUID = "fc9ba66c-642b-4720-9b45-a5889414a419",
            Title = "",
            Description = "",
            Price = 0,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 451,
            GUID = "2a51b2e7-b4a2-477a-90a2-cd17045de329",
            Title = "",
            Description = "",
            Price = 20,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 452,
            GUID = "5ee7e9b7-3f42-49a8-8ba6-00e3c4e9da99",
            Title = "",
            Description = "",
            Price = 20,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 453,
            GUID = "ddc06afd-1f78-424f-92e1-25e74b5a3931",
            Title = "",
            Description = "",
            Price = 20,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 454,
            GUID = "5730f32f-738c-494a-8b05-ef86cc4cf366",
            Title = "",
            Description = "",
            Price = 30,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 455,
            GUID = "5410266e-7394-490f-9a02-4d1e359a6c11",
            Title = "",
            Description = "",
            Price = 30,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 456,
            GUID = "0174faaf-b9bc-46da-9be7-b2033e3dcca0",
            Title = "",
            Description = "",
            Price = 30,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 457,
            GUID = "8c1b570e-6566-460b-a508-429e81f90a47",
            Title = "",
            Description = "",
            Price = 30,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 458,
            GUID = "b3c9ebe8-9300-4a1c-93ba-8143f432aeff",
            Title = "",
            Description = "",
            Price = 30,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 459,
            GUID = "c8fbae86-9488-473f-8384-1c0afea183b1",
            Title = "",
            Description = "",
            Price = 30,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 460,
            GUID = "a2d22d6b-aaea-4e56-a0a4-7c2afb1965f7",
            Title = "",
            Description = "",
            Price = 30,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 461,
            GUID = "e671c6ba-3066-4105-b90f-b5704dec8c10",
            Title = "",
            Description = "",
            Price = 30,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 462,
            GUID = "079faf1f-27da-40c6-8454-8b93189aa39c",
            Title = "",
            Description = "",
            Price = 30,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 463,
            GUID = "a5d523c5-5903-4242-8dba-a925f749bcd0",
            Title = "",
            Description = "",
            Price = 30,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 464,
            GUID = "faa35696-c13a-4928-b033-0f2ce003c39f",
            Title = "",
            Description = "",
            Price = 30,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 465,
            GUID = "7ff9d7cd-fbb6-4156-b8aa-ff475a8af483",
            Title = "",
            Description = "",
            Price = 30,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 466,
            GUID = "26cf312f-ab34-4bb0-933b-3f7cc5ff7538",
            Title = "",
            Description = "",
            Price = 30,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 467,
            GUID = "beeb7512-5859-45f9-8b49-c34c33e97cb3",
            Title = "",
            Description = "",
            Price = 30,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 468,
            GUID = "7cabbec4-c3b3-408e-872d-9b9443d03d75",
            Title = "",
            Description = "",
            Price = 30,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 469,
            GUID = "424c4ba0-d582-4bf5-869b-e81ab07c0ac8",
            Title = "",
            Description = "",
            Price = 30,
            VisibleInShop = true,
        }
    };

    public static ShopItemData[] Girl_Duck_Items = new ShopItemData[]
    {
        new ShopItemData
        {
            Id = 470,
            GUID = "b072deda-2c01-428e-865e-9c83f8b55c12",
            Title = "",
            Description = "",
            Price = 0,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 471,
            GUID = "33eca0d4-6495-47f0-a83e-37e300a1b9b5",
            Title = "",
            Description = "",
            Price = 0,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 472,
            GUID = "6091c41d-b4e4-40a2-b092-41811fd7c970",
            Title = "",
            Description = "",
            Price = 0,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 473,
            GUID = "7c79fa24-b047-4346-946e-fa163644d2c6",
            Title = "",
            Description = "",
            Price = 0,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 474,
            GUID = "7f6daf13-8ae1-4e57-845e-314f1b31b783",
            Title = "",
            Description = "",
            Price = 0,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 475,
            GUID = "66b46efe-2160-4c1b-84fd-76cd7918998b",
            Title = "",
            Description = "",
            Price = 0,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 476,
            GUID = "0af2e801-9cd2-4890-ac8f-0c07449e7869",
            Title = "",
            Description = "",
            Price = 0,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 477,
            GUID = "6ba3060e-af08-4c90-a568-e6d56a61232c",
            Title = "",
            Description = "",
            Price = 0,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 478,
            GUID = "b6c030e5-16ad-4ced-bc82-ca82ff412582",
            Title = "",
            Description = "",
            Price = 0,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 479,
            GUID = "8fb530d6-6b01-459b-a2c3-e6e94cce7d69",
            Title = "",
            Description = "",
            Price = 40,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 480,
            GUID = "739cf2bd-87eb-4129-a6d2-5d5e585a84f2",
            Title = "",
            Description = "",
            Price = 40,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 481,
            GUID = "83315cf6-1625-42eb-a142-936825d8ecbe",
            Title = "",
            Description = "",
            Price = 40,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 482,
            GUID = "53da362b-2299-460d-9f5d-b20280ceb2ed",
            Title = "",
            Description = "",
            Price = 25,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 483,
            GUID = "cae8cf27-a97f-4e3a-8adb-e5e26fcb9eb0",
            Title = "",
            Description = "",
            Price = 25,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 484,
            GUID = "30cc69a6-e9e5-458e-ba2d-26839122ae6b",
            Title = "",
            Description = "",
            Price = 25,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 485,
            GUID = "11b52005-86a0-4e4b-965f-de4c3daaafa5",
            Title = "",
            Description = "",
            Price = 25,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 486,
            GUID = "1cefdb8c-5d55-48a9-bc08-3c1615877eaa",
            Title = "",
            Description = "",
            Price = 40,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 487,
            GUID = "47eb9349-80d9-40a2-9b10-7d93ecbe495e",
            Title = "",
            Description = "",
            Price = 40,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 488,
            GUID = "f546cd89-31d1-427e-8810-e06d3aabe8f5",
            Title = "",
            Description = "",
            Price = 40,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 489,
            GUID = "0586a56c-882a-4072-8493-6d8dd40c84cb",
            Title = "",
            Description = "",
            Price = 40,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 490,
            GUID = "c096a071-c180-40b5-8ab1-7abebbbe9e9f",
            Title = "",
            Description = "",
            Price = 10,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 491,
            GUID = "e0c5ea8f-57be-4ff8-8c4e-726c000d1923",
            Title = "",
            Description = "",
            Price = 10,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 492,
            GUID = "a3d39562-61bd-41a3-960d-975a081a2902",
            Title = "",
            Description = "",
            Price = 10,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 493,
            GUID = "932b8dbd-c059-4379-8515-2f1ed6c24021",
            Title = "",
            Description = "",
            Price = 10,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 494,
            GUID = "f39b4612-164f-4c0d-90bb-050e368c29b9",
            Title = "",
            Description = "",
            Price = 10,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 495,
            GUID = "48257a1a-7077-4364-a7a5-c25f205ed46a",
            Title = "",
            Description = "",
            Price = 10,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 496,
            GUID = "f9963303-f2c3-4924-bb3f-c567d6c27c29",
            Title = "",
            Description = "",
            Price = 10,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 497,
            GUID = "1c35dba4-6167-4240-b5d5-597cc8f5b7bc",
            Title = "",
            Description = "",
            Price = 10,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 498,
            GUID = "d33cab63-aca1-49fb-bead-cfee55975b29",
            Title = "",
            Description = "",
            Price = 10,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 499,
            GUID = "d72137a4-884c-48c7-9886-6d89fdeafa73",
            Title = "",
            Description = "",
            Price = 10,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 500,
            GUID = "86ce7ce3-01f2-4a50-9925-d3bdf4045d61",
            Title = "",
            Description = "",
            Price = 10,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 501,
            GUID = "27ada100-363d-409e-9847-ef7b5cb08024",
            Title = "",
            Description = "",
            Price = 10,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 502,
            GUID = "895d8f1a-cde5-4bae-80db-54dbc70d757b",
            Title = "",
            Description = "",
            Price = 10,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 503,
            GUID = "c1830276-8b79-40e2-9e4c-0ec77362fdbe",
            Title = "",
            Description = "",
            Price = 10,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 504,
            GUID = "d2225996-6aab-4a2d-a0c0-ce50d7dc1ff8",
            Title = "",
            Description = "",
            Price = 10,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 505,
            GUID = "12cc578a-aa1a-436b-89b2-20981de04ec4",
            Title = "",
            Description = "",
            Price = 10,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 506,
            GUID = "47630234-4e7b-44bc-b757-3d31893255e9",
            Title = "",
            Description = "",
            Price = 10,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 507,
            GUID = "6e865156-abbb-48b2-8a3c-a6daa07e4e8c",
            Title = "",
            Description = "",
            Price = 10,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 508,
            GUID = "88fe1635-b520-43ce-ab64-a02eb3e5d381",
            Title = "",
            Description = "",
            Price = 10,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 509,
            GUID = "95c1ead7-caa0-469c-8193-8c15f1f35b6e",
            Title = "",
            Description = "",
            Price = 10,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 510,
            GUID = "121b78fa-ff3d-4ad8-8503-d5edb11e24a6",
            Title = "",
            Description = "",
            Price = 10,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 511,
            GUID = "a520666a-27a3-4df2-8fe2-9975c422079e",
            Title = "",
            Description = "",
            Price = 10,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 512,
            GUID = "39b76aab-2ebc-4c04-92a7-06640dae511b",
            Title = "",
            Description = "",
            Price = 10,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 513,
            GUID = "cd566613-f77f-4f4f-8332-626c92601498",
            Title = "",
            Description = "",
            Price = 10,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 514,
            GUID = "90cf7deb-5bcd-4962-970c-aec3cae0cdc6",
            Title = "",
            Description = "",
            Price = 10,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 515,
            GUID = "0b0dc84c-5e7b-4868-a62d-3b72472bb9a8",
            Title = "",
            Description = "",
            Price = 10,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 516,
            GUID = "e56adeae-3d6e-4463-af11-13856db7ff41",
            Title = "",
            Description = "",
            Price = 10,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 517,
            GUID = "f7f6c19f-fa7f-474a-8a03-b59aa2334d07",
            Title = "",
            Description = "",
            Price = 10,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 518,
            GUID = "4d7031b4-e266-415d-b7c9-29056b38c02f",
            Title = "",
            Description = "",
            Price = 999999999,
            VisibleInShop = false,
        },
        new ShopItemData
        {
            Id = 519,
            GUID = "28cfc25a-b137-4301-8de8-559213868f9b",
            Title = "",
            Description = "",
            Price = 999999999,
            VisibleInShop = false,
        },
        new ShopItemData
        {
            Id = 520,
            GUID = "2911d3e3-f424-448a-b7fd-6860ae6fc2f5",
            Title = "",
            Description = "",
            Price = 10,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 521,
            GUID = "141c9df2-3c84-4d2e-aa89-9a84e2011ccf",
            Title = "",
            Description = "",
            Price = 10,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 522,
            GUID = "44159b30-98e1-40f7-b1f9-5d90688755ff",
            Title = "",
            Description = "",
            Price = 10,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 523,
            GUID = "33f4ac43-cd6e-4158-88b3-7f45f582cbe4",
            Title = "",
            Description = "",
            Price = 10,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 524,
            GUID = "a4c30493-1e29-4fb1-9b9b-071ad01f3a0f",
            Title = "",
            Description = "",
            Price = 25,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 525,
            GUID = "e704995e-e527-4a8b-bba2-cebc2624cb0e",
            Title = "",
            Description = "",
            Price = 25,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 526,
            GUID = "0eeeaa24-3565-41f4-b3d6-a9020aaebf9a",
            Title = "",
            Description = "",
            Price = 25,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 527,
            GUID = "bf95cd2e-004c-437a-b50d-fb1d63702c37",
            Title = "",
            Description = "",
            Price = 25,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 528,
            GUID = "24f37cc2-d5fe-4a4b-b497-cf390a05c714",
            Title = "",
            Description = "",
            Price = 25,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 529,
            GUID = "740aaa0d-c242-49df-99bc-e7ac5951909b",
            Title = "",
            Description = "",
            Price = 25,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 530,
            GUID = "0904f1e7-bf40-45a9-b5ef-90c8fda85751",
            Title = "",
            Description = "",
            Price = 25,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 531,
            GUID = "61233702-7d03-4045-9d1e-170ba806d84a",
            Title = "",
            Description = "",
            Price = 25,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 532,
            GUID = "2ca31914-fb1f-495c-8222-c2ff219ae191",
            Title = "",
            Description = "",
            Price = 25,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 533,
            GUID = "dd5b1c85-3fd1-463f-a242-c1b08ac8058e",
            Title = "",
            Description = "",
            Price = 25,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 534,
            GUID = "e0bae540-26b2-4e66-bd82-9b866e347428",
            Title = "",
            Description = "",
            Price = 25,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 535,
            GUID = "6515627a-6a89-43f2-b7e1-06fd2bf106db",
            Title = "",
            Description = "",
            Price = 25,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 536,
            GUID = "a21f4126-fb76-4648-9e4c-04337e8a9b35",
            Title = "",
            Description = "",
            Price = 40,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 537,
            GUID = "3e4bdd33-3c0d-49d2-9535-4de8a0d88f11",
            Title = "",
            Description = "",
            Price = 40,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 538,
            GUID = "8caf64f8-0d08-4301-92a9-b34cb8d0891f",
            Title = "",
            Description = "",
            Price = 40,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 539,
            GUID = "143625b6-8fb9-4f47-bb46-9eb72b53329e",
            Title = "",
            Description = "",
            Price = 40,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 540,
            GUID = "f1ca5003-9824-43b8-bcfd-da5947cad411",
            Title = "",
            Description = "",
            Price = 0,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 541,
            GUID = "085bca46-26ed-4388-b114-39e615187e7c",
            Title = "",
            Description = "",
            Price = 25,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 542,
            GUID = "55ea9442-1b88-42ec-b182-551163905228",
            Title = "",
            Description = "",
            Price = 25,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 543,
            GUID = "6b749096-ea06-4ce3-9a09-9a90cc7718dd",
            Title = "",
            Description = "",
            Price = 25,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 544,
            GUID = "fa341e2f-931c-4282-aa87-8e3ecaa274e9",
            Title = "",
            Description = "",
            Price = 25,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 545,
            GUID = "87cad029-4cc5-4530-9deb-ef0074896c0e",
            Title = "",
            Description = "",
            Price = 25,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 546,
            GUID = "c8be6cc1-a820-49ae-a187-5468ef83370c",
            Title = "",
            Description = "",
            Price = 25,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 547,
            GUID = "3a2e4ae1-e09c-4142-abd5-9715c73b9a43",
            Title = "",
            Description = "",
            Price = 25,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 548,
            GUID = "ce450f6c-17f3-4535-9e8d-f4ec68184de9",
            Title = "",
            Description = "",
            Price = 10,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 549,
            GUID = "9a1415b7-7988-403f-b9b4-5ec0ebd0c6be",
            Title = "",
            Description = "",
            Price = 10,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 550,
            GUID = "7ce5bbee-1326-408b-b7ba-4f9e9375c34f",
            Title = "",
            Description = "",
            Price = 10,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 551,
            GUID = "00ffa031-5422-4044-affb-15909b3c0cc2",
            Title = "",
            Description = "",
            Price = 10,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 552,
            GUID = "c376e7d6-840a-4586-8d98-2339a06de6a4",
            Title = "",
            Description = "",
            Price = 50,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 553,
            GUID = "ef6e8792-4737-42f6-b240-3d0d2a0d2a87",
            Title = "",
            Description = "",
            Price = 50,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 554,
            GUID = "32088dcb-b447-4747-81f8-aad018f60a5c",
            Title = "",
            Description = "",
            Price = 50,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 555,
            GUID = "1f0ef55f-f213-448f-aeb3-e23c4fb2eb60",
            Title = "",
            Description = "",
            Price = 50,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 556,
            GUID = "f988b6ee-ebf6-44de-bbdd-c3c3fce82724",
            Title = "",
            Description = "",
            Price = 50,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 557,
            GUID = "4b967153-0859-4c0b-b1a3-e1e20bc5a92f",
            Title = "",
            Description = "",
            Price = 50,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 558,
            GUID = "da273b51-4e8d-4626-ad5e-8b1789946b96",
            Title = "",
            Description = "",
            Price = 50,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 559,
            GUID = "ee1e6899-9293-47df-a28e-cec347fb63bd",
            Title = "",
            Description = "",
            Price = 50,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 560,
            GUID = "0bc51851-d8aa-4d69-b24a-27ce23e992cc",
            Title = "",
            Description = "",
            Price = 50,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 561,
            GUID = "b485c25d-b329-4522-b388-172fe7d65870",
            Title = "",
            Description = "",
            Price = 50,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 562,
            GUID = "2e5a29c0-d630-49a0-988b-20c37f50b2d8",
            Title = "",
            Description = "",
            Price = 50,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 563,
            GUID = "c991e57b-66cb-4986-a2c1-a70ee1a87c57",
            Title = "",
            Description = "",
            Price = 50,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 564,
            GUID = "6a9d556a-3b28-480d-963c-8a8d729d38cb",
            Title = "",
            Description = "",
            Price = 50,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 565,
            GUID = "9f8c9786-28db-4282-bcf8-ccd959e6efc5",
            Title = "",
            Description = "",
            Price = 50,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 566,
            GUID = "78aa7064-636c-4e07-95ae-8329447f08e9",
            Title = "",
            Description = "",
            Price = 50,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 567,
            GUID = "e2bad881-6030-4c78-bf6d-67d98eeae420",
            Title = "",
            Description = "",
            Price = 50,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 568,
            GUID = "495a3c63-4f60-4613-a626-83d474165cdf",
            Title = "",
            Description = "",
            Price = 0,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 569,
            GUID = "06c00388-e86c-4683-8cc1-6cba31e3ca35",
            Title = "",
            Description = "",
            Price = 20,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 570,
            GUID = "f8db1515-bafa-460a-8cf3-9643bb588a1c",
            Title = "",
            Description = "",
            Price = 20,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 571,
            GUID = "12bc4c67-5429-4c8a-80d8-eae7e0b098b9",
            Title = "",
            Description = "",
            Price = 20,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 572,
            GUID = "3e913ee0-1998-4a39-b664-7c252b9937ee",
            Title = "",
            Description = "",
            Price = 30,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 573,
            GUID = "ef0e2438-be2f-480c-8ec5-afaa337a155b",
            Title = "",
            Description = "",
            Price = 30,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 574,
            GUID = "a0b774b1-5fea-4744-8714-2b1adaf2d715",
            Title = "",
            Description = "",
            Price = 30,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 575,
            GUID = "1278912c-398e-4601-8828-99f375775767",
            Title = "",
            Description = "",
            Price = 30,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 576,
            GUID = "1829c496-90aa-4ddd-a64c-31df3ec710d4",
            Title = "",
            Description = "",
            Price = 30,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 577,
            GUID = "f0d8b247-5927-432d-a14e-d8a09c03f7ce",
            Title = "",
            Description = "",
            Price = 30,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 578,
            GUID = "babd03ce-5c73-48e4-a0be-a3bf74ee4d86",
            Title = "",
            Description = "",
            Price = 30,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 579,
            GUID = "5cde37a4-9fba-4529-b340-ffbc52785080",
            Title = "",
            Description = "",
            Price = 30,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 580,
            GUID = "19c62432-ab02-4d96-9557-9fab561213d7",
            Title = "",
            Description = "",
            Price = 30,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 581,
            GUID = "232596ab-155b-45d3-81c7-973a554d6c10",
            Title = "",
            Description = "",
            Price = 30,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 582,
            GUID = "3ea469fe-0fd9-4173-a0d3-62e5a18827b9",
            Title = "",
            Description = "",
            Price = 30,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 583,
            GUID = "a6b01493-12e3-4ced-a5f9-1d410a48fc83",
            Title = "",
            Description = "",
            Price = 30,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 584,
            GUID = "95720cb0-202c-4b54-bd01-98c40adb17f2",
            Title = "",
            Description = "",
            Price = 30,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 585,
            GUID = "c3c3aa77-c131-4990-a1bb-82b439939911",
            Title = "",
            Description = "",
            Price = 30,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 586,
            GUID = "064146e8-5a61-4405-b5dd-632549c44a1d",
            Title = "",
            Description = "",
            Price = 30,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 587,
            GUID = "109366f9-05ab-4094-a64e-07583caa17fe",
            Title = "",
            Description = "",
            Price = 30,
            VisibleInShop = true,
        }
    };

    public static ShopItemData[] Girl_Cat_Items = new ShopItemData[]
    {
        new ShopItemData
        {
            Id = 588,
            GUID = "a459102a-e924-416a-ab3f-28356cda329a",
            Title = "",
            Description = "",
            Price = 0,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 589,
            GUID = "04aa3ed7-6974-4fbd-9bab-3c98a731515a",
            Title = "",
            Description = "",
            Price = 0,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 590,
            GUID = "be89f318-7478-4088-8759-24171c249fba",
            Title = "",
            Description = "",
            Price = 0,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 591,
            GUID = "59a16709-5038-4279-8122-582684705aa1",
            Title = "",
            Description = "",
            Price = 0,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 592,
            GUID = "b8def86a-b0c1-4711-8bc6-1900b26aa790",
            Title = "",
            Description = "",
            Price = 0,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 593,
            GUID = "4f5797df-e7fc-46bd-8a10-379ff817145d",
            Title = "",
            Description = "",
            Price = 0,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 594,
            GUID = "ff42265c-98cd-4f4a-9c6f-7f6dbd89f9bc",
            Title = "",
            Description = "",
            Price = 0,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 595,
            GUID = "13568c2c-6638-4da4-8aa6-93711b7412f0",
            Title = "",
            Description = "",
            Price = 0,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 596,
            GUID = "77684941-57e4-4929-9048-c475cd6275d8",
            Title = "",
            Description = "",
            Price = 0,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 597,
            GUID = "bcf691ae-5a14-4101-9e3a-7ac914a33b0a",
            Title = "",
            Description = "",
            Price = 40,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 598,
            GUID = "2a9db40b-edab-4c2d-846e-defa502c4be2",
            Title = "",
            Description = "",
            Price = 40,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 599,
            GUID = "946d020d-30c4-4128-a488-462b7cc9a1de",
            Title = "",
            Description = "",
            Price = 40,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 600,
            GUID = "8272b039-82de-4243-ae51-67fa97631dff",
            Title = "",
            Description = "",
            Price = 25,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 601,
            GUID = "c8a10342-4678-4b40-8347-32bd5f5c1d56",
            Title = "",
            Description = "",
            Price = 25,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 602,
            GUID = "82ea4fdc-787c-495c-8a84-9dc50997fc41",
            Title = "",
            Description = "",
            Price = 25,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 603,
            GUID = "21ba956b-d0b6-4abb-9f34-75cd0912de84",
            Title = "",
            Description = "",
            Price = 25,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 604,
            GUID = "bcf4ce1c-d014-4667-8f3c-e7388412127f",
            Title = "",
            Description = "",
            Price = 40,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 605,
            GUID = "8206f3f3-3a0b-457a-abb0-bf57baf42a76",
            Title = "",
            Description = "",
            Price = 40,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 606,
            GUID = "fcb50c58-d8bf-47e1-b8fa-3931f32bc5a9",
            Title = "",
            Description = "",
            Price = 40,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 607,
            GUID = "dd290a41-4d61-417f-9f82-2dda03741977",
            Title = "",
            Description = "",
            Price = 40,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 608,
            GUID = "7ec4f424-de63-4ebc-ac10-bc2599b55799",
            Title = "",
            Description = "",
            Price = 10,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 609,
            GUID = "47f3d859-3204-4681-b2e5-1273b5beea9f",
            Title = "",
            Description = "",
            Price = 10,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 610,
            GUID = "b7640ecb-1c3c-4488-bed1-48968a8629ae",
            Title = "",
            Description = "",
            Price = 10,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 611,
            GUID = "c1151e77-04bb-4c78-9f0c-c4841b78a19e",
            Title = "",
            Description = "",
            Price = 10,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 612,
            GUID = "00f0056b-316c-466e-8965-f13ecf253013",
            Title = "",
            Description = "",
            Price = 10,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 613,
            GUID = "3d5c0ee2-0111-4d45-ace8-1c96a4f30c81",
            Title = "",
            Description = "",
            Price = 10,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 614,
            GUID = "2aff109b-5e3f-438f-9959-38e5cf33e258",
            Title = "",
            Description = "",
            Price = 10,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 615,
            GUID = "41f36726-642a-44a0-849f-bc1672de14d2",
            Title = "",
            Description = "",
            Price = 10,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 616,
            GUID = "261e975e-32b5-4363-b6b9-532bfd97106a",
            Title = "",
            Description = "",
            Price = 10,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 617,
            GUID = "f6ad4cbe-8353-4fe4-96c9-803b8189edc6",
            Title = "",
            Description = "",
            Price = 10,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 618,
            GUID = "5acd23ca-5e08-4ecf-a248-23416686a113",
            Title = "",
            Description = "",
            Price = 10,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 619,
            GUID = "3680a18c-fc6e-48d0-8742-c3e0dd8aed31",
            Title = "",
            Description = "",
            Price = 10,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 620,
            GUID = "e58eca24-d7c5-4cd5-9141-19c1c037083a",
            Title = "",
            Description = "",
            Price = 10,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 621,
            GUID = "b66a56d8-f70e-4f8a-80c0-425941a73753",
            Title = "",
            Description = "",
            Price = 10,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 622,
            GUID = "c3858c81-629e-438f-a278-cf7e6c37e011",
            Title = "",
            Description = "",
            Price = 10,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 623,
            GUID = "87b5e793-25da-4648-bbde-d53dc20087cd",
            Title = "",
            Description = "",
            Price = 10,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 624,
            GUID = "32e3c8db-e814-4727-8d26-49cc21adcc65",
            Title = "",
            Description = "",
            Price = 10,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 625,
            GUID = "efadf560-5184-4ce1-ba95-fe87ed6343cb",
            Title = "",
            Description = "",
            Price = 10,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 626,
            GUID = "dfb98ba8-99f4-4933-b013-554b15c2f6cd",
            Title = "",
            Description = "",
            Price = 10,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 627,
            GUID = "bd5e9d43-cd78-4f62-b649-5032505b1ab8",
            Title = "",
            Description = "",
            Price = 10,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 628,
            GUID = "9c38f9d9-0ee4-454d-9cfc-f6831467f89d",
            Title = "",
            Description = "",
            Price = 10,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 629,
            GUID = "2491144f-f39b-4c4f-a7eb-d1743b4d556c",
            Title = "",
            Description = "",
            Price = 10,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 630,
            GUID = "0e8d81f9-c4ca-4264-8f7b-8d43efa564b2",
            Title = "",
            Description = "",
            Price = 10,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 631,
            GUID = "9c5097c3-d48a-4514-b562-8c0e6d408f8d",
            Title = "",
            Description = "",
            Price = 10,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 632,
            GUID = "9bf83764-7d97-479b-9355-ff5efb055ac1",
            Title = "",
            Description = "",
            Price = 10,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 633,
            GUID = "937af964-f3b0-4385-b091-6026d5369217",
            Title = "",
            Description = "",
            Price = 10,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 634,
            GUID = "0b7f478a-47ca-42b4-9046-bb10d8d4183e",
            Title = "",
            Description = "",
            Price = 10,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 635,
            GUID = "c42365cb-40d4-4660-bde7-016848f94124",
            Title = "",
            Description = "",
            Price = 10,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 636,
            GUID = "97907e20-b70f-4769-badc-05dfed67e46d",
            Title = "",
            Description = "",
            Price = 999999999,
            VisibleInShop = false,
        },
        new ShopItemData
        {
            Id = 637,
            GUID = "f988b9d5-5c68-45eb-b2c2-05a3c9aa4085",
            Title = "",
            Description = "",
            Price = 999999999,
            VisibleInShop = false,
        },
        new ShopItemData
        {
            Id = 638,
            GUID = "0f797994-84c0-4dde-a8ff-28d6415dd190",
            Title = "",
            Description = "",
            Price = 10,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 639,
            GUID = "0524aa45-1341-4fc8-9d9d-861483af530c",
            Title = "",
            Description = "",
            Price = 10,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 640,
            GUID = "2d83b2eb-c30e-48f6-b959-0e6d363c56b7",
            Title = "",
            Description = "",
            Price = 10,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 641,
            GUID = "c979ba2a-172d-4489-a89b-4123db35e8d1",
            Title = "",
            Description = "",
            Price = 10,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 642,
            GUID = "0231d26b-00a6-4cc3-addd-e154b2ae1a4d",
            Title = "",
            Description = "",
            Price = 25,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 643,
            GUID = "3471642a-eac0-47c5-8ffa-a4e798be7030",
            Title = "",
            Description = "",
            Price = 25,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 644,
            GUID = "81424a9c-60c7-4cdb-8bd2-c3f860822eaa",
            Title = "",
            Description = "",
            Price = 25,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 645,
            GUID = "664bd2eb-43fd-49b2-b6d7-55ba9d86210f",
            Title = "",
            Description = "",
            Price = 25,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 646,
            GUID = "3bda7d60-2101-4d1d-859f-a7b8ddffaa61",
            Title = "",
            Description = "",
            Price = 25,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 647,
            GUID = "ef7f5fab-0c2b-4315-929c-52fb2507e4e5",
            Title = "",
            Description = "",
            Price = 25,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 648,
            GUID = "2adf21e9-b545-4480-ad8e-130338ca76df",
            Title = "",
            Description = "",
            Price = 25,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 649,
            GUID = "7fc69c76-847c-41c9-915b-bdec2dba4a75",
            Title = "",
            Description = "",
            Price = 25,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 650,
            GUID = "bd2403da-da5e-412c-a00d-d4693e312191",
            Title = "",
            Description = "",
            Price = 25,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 651,
            GUID = "fb74bc1a-dc26-4b85-80d0-170c3825313c",
            Title = "",
            Description = "",
            Price = 25,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 652,
            GUID = "fe61d8d6-affb-4a18-8cb6-7d5bcc9d7369",
            Title = "",
            Description = "",
            Price = 25,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 653,
            GUID = "613cff2d-48aa-49a8-b08a-2ec9556afcc6",
            Title = "",
            Description = "",
            Price = 25,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 654,
            GUID = "c39f9d23-c930-4568-9804-2e6e71f416e2",
            Title = "",
            Description = "",
            Price = 40,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 655,
            GUID = "df8d7c7c-bf4b-4a73-8a32-bb3739984d7b",
            Title = "",
            Description = "",
            Price = 40,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 656,
            GUID = "8d77c7ef-704d-450d-a56d-5d8727769e2b",
            Title = "",
            Description = "",
            Price = 40,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 657,
            GUID = "ee69b2f0-9e12-4d6d-9463-17a6ab55d7da",
            Title = "",
            Description = "",
            Price = 40,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 658,
            GUID = "9a93a7fc-f5a3-483c-b0cb-5c7cbf051e30",
            Title = "",
            Description = "",
            Price = 0,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 659,
            GUID = "015c7cc0-6c5c-46a6-b09c-e0e12a00ad47",
            Title = "",
            Description = "",
            Price = 25,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 660,
            GUID = "5e761cb0-ff3b-430c-bb01-960d76b8a666",
            Title = "",
            Description = "",
            Price = 25,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 661,
            GUID = "e8272845-f5af-447d-ae64-21f261cf9763",
            Title = "",
            Description = "",
            Price = 25,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 662,
            GUID = "c2adc3ef-8c5a-4a35-8291-d8907a2f8d32",
            Title = "",
            Description = "",
            Price = 25,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 663,
            GUID = "4d9db3b5-3158-4f5d-b8d2-a23bf8bbf45f",
            Title = "",
            Description = "",
            Price = 25,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 664,
            GUID = "b83affdc-4d1d-4acb-87f4-abe8b2af6994",
            Title = "",
            Description = "",
            Price = 25,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 665,
            GUID = "41f26025-a0a7-411c-b342-96064c6a968c",
            Title = "",
            Description = "",
            Price = 25,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 666,
            GUID = "68204e8c-53ea-4f76-860f-cf4dd63e6605",
            Title = "",
            Description = "",
            Price = 10,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 667,
            GUID = "ae37e630-877c-44c9-bbac-145818737af6",
            Title = "",
            Description = "",
            Price = 10,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 668,
            GUID = "9f3582a4-4e8e-4b60-8ed5-2e1c984065ca",
            Title = "",
            Description = "",
            Price = 10,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 669,
            GUID = "149a3b87-85a2-4a80-ab5b-b23ec21033d9",
            Title = "",
            Description = "",
            Price = 10,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 670,
            GUID = "d95c084f-ba35-4c13-ac9f-bbb08ea058f7",
            Title = "",
            Description = "",
            Price = 50,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 671,
            GUID = "94e7c36d-4688-42f9-9cdf-7d7df4813554",
            Title = "",
            Description = "",
            Price = 50,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 672,
            GUID = "5fa5531d-072b-4e69-89f7-fe7fe6af5171",
            Title = "",
            Description = "",
            Price = 50,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 673,
            GUID = "1c0e6333-f2bd-490a-b821-79f56989c13e",
            Title = "",
            Description = "",
            Price = 50,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 674,
            GUID = "22417c7a-3328-4cef-95fe-907f75be162d",
            Title = "",
            Description = "",
            Price = 50,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 675,
            GUID = "4818df68-15be-4072-a4f3-e51ef7ee31cc",
            Title = "",
            Description = "",
            Price = 50,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 676,
            GUID = "cc9a7f6a-84f1-4d97-b918-b1ac05c0bb77",
            Title = "",
            Description = "",
            Price = 50,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 677,
            GUID = "16adaa09-f9ef-49c3-b885-200cc26e2d25",
            Title = "",
            Description = "",
            Price = 50,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 678,
            GUID = "c2ebb500-fbe4-4ba5-9e8a-1c325b5a5651",
            Title = "",
            Description = "",
            Price = 50,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 679,
            GUID = "1e78ef58-5c41-4311-bd5a-88d7d3037213",
            Title = "",
            Description = "",
            Price = 50,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 680,
            GUID = "9ddec072-391f-41ce-a302-9397d8fd5f9b",
            Title = "",
            Description = "",
            Price = 50,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 681,
            GUID = "48d703de-bbd8-4082-ad24-2be5c9132bbd",
            Title = "",
            Description = "",
            Price = 50,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 682,
            GUID = "7193413f-3a6a-47ff-b60b-63b435104120",
            Title = "",
            Description = "",
            Price = 50,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 683,
            GUID = "a202a95d-942d-4be5-9414-16994b699f85",
            Title = "",
            Description = "",
            Price = 50,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 684,
            GUID = "181d2eef-ed7f-4471-953c-cf85ad72d393",
            Title = "",
            Description = "",
            Price = 50,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 685,
            GUID = "c1fe1d96-fa49-4ef0-8837-359f5fa0d5e1",
            Title = "",
            Description = "",
            Price = 50,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 686,
            GUID = "208e06c8-fc63-4429-b57b-9b4e5a8f0bfd",
            Title = "",
            Description = "",
            Price = 0,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 687,
            GUID = "b60a6af3-b905-4a7c-bb40-d1654ae1bb24",
            Title = "",
            Description = "",
            Price = 20,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 688,
            GUID = "970718b5-7ff8-4710-8499-866bffbfbbb5",
            Title = "",
            Description = "",
            Price = 20,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 689,
            GUID = "70366ff7-1167-4ffb-9b86-d3e4e64e9c59",
            Title = "",
            Description = "",
            Price = 20,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 690,
            GUID = "890ee7f3-e722-4e3d-8728-88edf2484c59",
            Title = "",
            Description = "",
            Price = 30,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 691,
            GUID = "6b571543-0e21-4f96-b334-703a5d01b52f",
            Title = "",
            Description = "",
            Price = 30,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 692,
            GUID = "6416212c-fb55-4a10-956d-05e1cb046105",
            Title = "",
            Description = "",
            Price = 30,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 693,
            GUID = "fc9d2951-b5cd-4496-8520-7e7e285d64fe",
            Title = "",
            Description = "",
            Price = 30,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 694,
            GUID = "ba64c7dc-9483-431a-9bb2-2dc31b2c00c9",
            Title = "",
            Description = "",
            Price = 30,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 695,
            GUID = "9d35b1bc-6f1b-4ff3-9ab4-def7a6a1ad1c",
            Title = "",
            Description = "",
            Price = 30,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 696,
            GUID = "e1a663ad-30cf-41d4-aac1-aa02c4f9f118",
            Title = "",
            Description = "",
            Price = 30,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 697,
            GUID = "669d1ca6-fd16-47e9-916c-efccd1435d11",
            Title = "",
            Description = "",
            Price = 30,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 698,
            GUID = "fae33ac4-de72-4855-8c36-ed22c97d789a",
            Title = "",
            Description = "",
            Price = 30,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 699,
            GUID = "6f9fc7af-e05c-4f71-9915-565e81a0e304",
            Title = "",
            Description = "",
            Price = 30,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 700,
            GUID = "9d259881-da72-49ee-8a00-479078be32ee",
            Title = "",
            Description = "",
            Price = 30,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 701,
            GUID = "2392e202-01a9-40cb-93fe-efac41dfaa00",
            Title = "",
            Description = "",
            Price = 30,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 702,
            GUID = "7de53184-9742-466f-bd78-b23ef5b2224f",
            Title = "",
            Description = "",
            Price = 30,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 703,
            GUID = "d77c5490-e2c5-4d57-a697-bad0abae025c",
            Title = "",
            Description = "",
            Price = 30,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 704,
            GUID = "57212a9f-5a88-40e7-b733-8040471a1b20",
            Title = "",
            Description = "",
            Price = 30,
            VisibleInShop = true,
        },
        new ShopItemData
        {
            Id = 705,
            GUID = "7c6a3c2c-73b8-4e49-9c41-e0c0ebef9f55",
            Title = "",
            Description = "",
            Price = 30,
            VisibleInShop = true,
        }
    };
}