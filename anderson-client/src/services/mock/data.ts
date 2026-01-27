export enum Region {
  NorthAmerica = "North America",
  Europe = "Europe",
  Africa = "Africa",
  AsiaPacific = "Asia Pacific",
  LatinAmerica = "Latin America",
}

export interface CountryEntry {
  name: string;
  region: string;
}

export interface Contact {
  name: string;
  email: string;
  isDefault: boolean;
}

export interface LocationEntry {
  region: Region;
  country: string;
  isHeadOffice: boolean;
}

export interface PartnerUser {
  email: string;
  firstName: string;
  lastName: string;
  title: string;
}

export interface Review {
  rating: number; // 1-5
  comment: string;
  date?: string;
  reviewerName?: string;
}

export interface Partner {
  id: string;
  name: string;
  website: string;
  description: string;
  serviceType: string;
  skills: string[];
  industries: string[];
  certifications: string[];
  matchScore?: number;
  verified: boolean;

  // New structured entities
  contacts: Contact[];
  locations: LocationEntry[];
  users: PartnerUser[];
  reviews: Review[];
}

export interface Opportunity {
  id: string;
  title: string;
  type: "Tender" | "Joint Venture" | "Product Launch" | "Resource Request";
  postedBy: string;
  partnerId?: string;
  deadline: string;
  description: string;
  fullDescription: string;
  requiredSkills: string[];
  status: "Open" | "Closed" | "In Review";
  region: Region;
  interestedPartnerIds?: string[];
}

export const MOCK_PARTNERS: Partner[] = [
  {
    id: "1",
    name: "Insight Consulting",
    website: "https://www.insightconsulting.co.za",
    description:
      "Leading data analytics and business intelligence firm specializing in Qlik and Snowflake implementations.",
    serviceType: "Advisory",
    skills: ["Qlik", "Snowflake", "Data Engineering", "Azure", "PowerBI"],
    industries: ["Retail", "Financial Services", "Manufacturing"],
    certifications: ["Qlik Elite Partner", "Microsoft Gold Data Analytics"],
    verified: true,
    locations: [
      { region: Region.Africa, country: "South Africa", isHeadOffice: true },
      { region: Region.Europe, country: "United Kingdom", isHeadOffice: false },
    ],
    contacts: [
      {
        name: "Sarah Johnson",
        email: "s.johnson@insight.example.com",
        isDefault: true,
      },
      {
        name: "David Smith",
        email: "d.smith@insight.example.com",
        isDefault: false,
      },
    ],
    users: [
      {
        email: "s.johnson@insight.example.com",
        firstName: "Sarah",
        lastName: "Johnson",
        title: "Managing Partner",
      },
      {
        email: "m.bekker@insight.example.com",
        firstName: "Mark",
        lastName: "Bekker",
        title: "Senior Data Architect",
      },
    ],
    reviews: [
      {
        rating: 5,
        comment: "Exceptional delivery on our Snowflake migration.",
        reviewerName: "Global Retail Corp",
        date: "2023-11-12",
      },
      {
        rating: 4,
        comment: "Strong technical skills, very responsive.",
        reviewerName: "Standard Bank",
        date: "2024-01-05",
      },
    ],
  },
  {
    id: "2",
    name: "Nordic Tech Solutions",
    website: "https://nordictech.example.com",
    description:
      "Specialists in cloud infrastructure migration and sustainable IT solutions.",
    serviceType: "IT Consulting",
    skills: ["AWS", "GCP", "Green IT", "Kubernetes", "DevOps"],
    industries: ["Energy", "Public Sector"],
    certifications: ["AWS Premier Partner", "ISO 14001"],
    verified: true,
    locations: [
      { region: Region.Europe, country: "Sweden", isHeadOffice: true },
      { region: Region.Europe, country: "Norway", isHeadOffice: false },
    ],
    contacts: [
      {
        name: "Lars Jensen",
        email: "l.jensen@nordictech.example.com",
        isDefault: true,
      },
    ],
    users: [
      {
        email: "l.jensen@nordictech.example.com",
        firstName: "Lars",
        lastName: "Jensen",
        title: "CEO",
      },
      {
        email: "e.nilsson@nordictech.example.com",
        firstName: "Erik",
        lastName: "Nilsson",
        title: "Cloud Lead",
      },
    ],
    reviews: [
      {
        rating: 5,
        comment: "Highly recommended for AWS migrations.",
        reviewerName: "Vattenfall",
        date: "2023-09-20",
      },
    ],
  },
  {
    id: "3",
    name: "LatAm FinTech Innovators",
    website: "https://latamfin.example.com",
    description:
      "Digital transformation experts focused on banking modernization and blockchain.",
    serviceType: "Financial Law",
    skills: ["Blockchain", "Java", "React", "Cybersecurity", "FinTech"],
    industries: ["Banking", "Insurance"],
    certifications: ["PCI DSS Compliant"],
    verified: true,
    locations: [
      { region: Region.LatinAmerica, country: "Brazil", isHeadOffice: true },
      { region: Region.LatinAmerica, country: "Mexico", isHeadOffice: false },
    ],
    contacts: [
      {
        name: "Maria Silva",
        email: "m.silva@latamfin.example.com",
        isDefault: true,
      },
    ],
    users: [
      {
        email: "m.silva@latamfin.example.com",
        firstName: "Maria",
        lastName: "Silva",
        title: "Director of Innovation",
      },
    ],
    reviews: [
      {
        rating: 5,
        comment: "Transformed our core banking system in record time.",
        reviewerName: "Itau Unibanco",
        date: "2023-12-01",
      },
    ],
  },
  {
    id: "4",
    name: "AsiaPac Logistics AI",
    website: "https://apaclogistics.example.com",
    description:
      "AI-driven supply chain optimization and predictive maintenance.",
    serviceType: "Supply Chain Advisory",
    skills: ["Python", "TensorFlow", "Supply Chain", "IoT"],
    industries: ["Logistics", "Automotive"],
    certifications: ["Google AI Partner"],
    verified: true,
    locations: [
      { region: Region.AsiaPacific, country: "Singapore", isHeadOffice: true },
    ],
    contacts: [
      {
        name: "Wei Chen",
        email: "w.chen@apaclogistics.example.com",
        isDefault: true,
      },
    ],
    users: [
      {
        email: "w.chen@apaclogistics.example.com",
        firstName: "Wei",
        lastName: "Chen",
        title: "Lead AI Scientist",
      },
    ],
    reviews: [
      {
        rating: 4,
        comment: "Great AI insights, helped us save 15% on fuel.",
        reviewerName: "DHL APAC",
        date: "2024-02-15",
      },
    ],
  },
  {
    id: "5",
    name: "EuroStrategy Group",
    website: "https://eurostrat.example.com",
    description:
      "Strategic management consulting with a focus on digital change management.",
    serviceType: "Tax Consulting",
    skills: ["Change Management", "Strategy", "SAP", "Project Management"],
    industries: ["Automotive", "Pharmaceuticals"],
    certifications: ["SAP Gold Partner"],
    verified: false,
    locations: [
      { region: Region.Europe, country: "Germany", isHeadOffice: true },
    ],
    contacts: [
      {
        name: "Hans Muller",
        email: "h.muller@eurostrat.example.com",
        isDefault: true,
      },
    ],
    users: [
      {
        email: "h.muller@eurostrat.example.com",
        firstName: "Hans",
        lastName: "Muller",
        title: "Senior Partner",
      },
    ],
    reviews: [],
  },
];

export const MOCK_OPPORTUNITIES: Opportunity[] = [
  {
    id: "101",
    title: "Global Retail Analytics Transformation",
    type: "Tender",
    postedBy: "Insight Consulting",
    partnerId: "1",
    deadline: "2024-06-30",
    description:
      "Seeking a partner with deep Snowflake and Qlik expertise to assist in a large-scale data migration and reporting overhaul for a UK retailer.",
    fullDescription:
      "We have secured a primary contract with a major UK high-street retailer to overhaul their legacy data warehousing solution. The project involves migrating 50TB of on-premise Oracle data to Snowflake. \n\nWe are looking for a partner to lead the data modeling and visualization stream. The end client utilizes Qlik Sense for all operational reporting. The engagement will last approximately 9 months, with a requirement for at least 2 senior consultants to be available during UK business hours. \n\nKey Responsibilities:\n- Design Snowflake schemas optimized for Qlik.\n- Rebuild 50+ mission-critical dashboards.\n- Conduct user training for the client's internal data team.",
    requiredSkills: ["Snowflake", "Qlik", "Retail Experience"],
    status: "Open",
    region: Region.Europe,
    interestedPartnerIds: ["2", "3", "4"],
  },
  {
    id: "102",
    title: "Pan-African Banking Security Audit",
    type: "Joint Venture",
    postedBy: "Insight Consulting",
    partnerId: "1",
    deadline: "2024-05-15",
    description:
      "Joint venture opportunity to deliver cybersecurity auditing services across 12 African countries.",
    fullDescription:
      "We are forming a consortium to bid for a pan-African banking compliance audit. The mandate covers 12 countries in West and East Africa. \n\nOur firm specializes in regulatory compliance (Basel III), but we require a technical security partner to handle the penetration testing and infrastructure audit portion of the tender. \n\nThe ideal partner must have prior experience working in the African banking sector and be able to deploy resources to Nigeria and Kenya for on-site assessments.",
    requiredSkills: ["Cybersecurity", "Auditing", "French Language"],
    status: "In Review",
    region: Region.Africa,
    interestedPartnerIds: ["4"],
  },
  {
    id: "103",
    title: "IoT Implementation for Smart Factory",
    type: "Resource Request",
    postedBy: "Nordic Tech Solutions",
    partnerId: "2",
    deadline: "2024-06-01",
    description:
      "Urgent need for 3 Senior IoT Engineers for a 6-month automotive project.",
    fullDescription:
      "We are currently 2 months into a digital twin project for a German automotive manufacturer. Due to scope expansion, we urgently require 3 Senior IoT Engineers to join the team. \n\nTech Stack:\n- Azure IoT Hub\n- C# / .NET Core on Edge devices\n- MQTT protocol\n\nThis is a staff augmentation arrangement. Resources can work remotely within +/- 2 hours of CET, but monthly site visits to Munich may be required.",
    requiredSkills: ["IoT", "Azure IoT Hub", "German Language"],
    status: "Open",
    region: Region.Europe,
    interestedPartnerIds: ["1", "5"],
  },
];
