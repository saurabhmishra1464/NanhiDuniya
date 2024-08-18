import { z } from "zod";

export const ResetPasswordFormValidation = z.object({
    newPassword: z
        .string()
        .min(8, { message: "Password must be at least 8 characters long" })
        .regex(/[a-z]/, { message: "Password must contain at least one lowercase letter" })
        .regex(/[A-Z]/, { message: "Password must contain at least one uppercase letter" })
        .regex(/[0-9]/, { message: "Password must contain at least one number" })
        .regex(/[@$!%*?&]/, { message: "Password must contain at least one special character" }),

    confirmPassword: z
        .string()
        .min(8, { message: "Password must be at least 8 characters long" })
        .regex(/[a-z]/, { message: "Password must contain at least one lowercase letter" })
        .regex(/[A-Z]/, { message: "Password must contain at least one uppercase letter" })
        .regex(/[0-9]/, { message: "Password must contain at least one number" })
        .regex(/[@$!%*?&]/, { message: "Password must contain at least one special character" }),
});

export const LoginFormValidation = z.object({
    userName: z
        .string()
        .nonempty({ message: 'Email is required' })
        .email({ message: 'Invalid email address' }),

    password: z
        .string()
        .min(8, { message: "Password must be at least 8 characters long" })
        .regex(/[a-z]/, { message: "Password must contain at least one lowercase letter" })
        .regex(/[A-Z]/, { message: "Password must contain at least one uppercase letter" })
        .regex(/[0-9]/, { message: "Password must contain at least one number" })
        .regex(/[@$!%*?&]/, { message: "Password must contain at least one special character" }),
});

export const PersonalInfoValidation = z.object({
    fullName: z.string().min(2, "Full name must be at least 2 characters")
        .max(50, "Full name must be at most 50 characters"),
    email: z.string().email("Invalid email address"),
    phoneNumber: z.string().transform((phone) => {
        // Add +91 if it is not already present
        if (!phone.startsWith('+91')) {
          return `+91${phone}`;
        }
        return phone;
      }).refine((phone) => /^\+\d{12}$/.test(phone), "Invalid phone number"),
    
    bio: z.string().optional(),

})

// export const UserFormValidation = z.object({
//   name: z
//     .string()
//     .min(2, "Name must be at least 2 characters")
//     .max(50, "Name must be at most 50 characters"),
//   email: z.string().email("Invalid email address"),
//   phone: z
//     .string()
//     .refine((phone) => /^\+\d{10,15}$/.test(phone), "Invalid phone number"),
// });

// export const PatientFormValidation = z.object({
//   name: z
//     .string()
//     .min(2, "Name must be at least 2 characters")
//     .max(50, "Name must be at most 50 characters"),
//   email: z.string().email("Invalid email address"),
//   phone: z
//     .string()
//     .refine((phone) => /^\+\d{10,15}$/.test(phone), "Invalid phone number"),
//   birthDate: z.coerce.date(),
//   gender: z.enum(["Male", "Female", "Other"]),
//   address: z
//     .string()
//     .min(5, "Address must be at least 5 characters")
//     .max(500, "Address must be at most 500 characters"),
//   occupation: z
//     .string()
//     .min(2, "Occupation must be at least 2 characters")
//     .max(500, "Occupation must be at most 500 characters"),
//   emergencyContactName: z
//     .string()
//     .min(2, "Contact name must be at least 2 characters")
//     .max(50, "Contact name must be at most 50 characters"),
//   emergencyContactNumber: z
//     .string()
//     .refine(
//       (emergencyContactNumber) => /^\+\d{10,15}$/.test(emergencyContactNumber),
//       "Invalid phone number"
//     ),
//   primaryPhysician: z.string().min(2, "Select at least one doctor"),
//   insuranceProvider: z
//     .string()
//     .min(2, "Insurance name must be at least 2 characters")
//     .max(50, "Insurance name must be at most 50 characters"),
//   insurancePolicyNumber: z
//     .string()
//     .min(2, "Policy number must be at least 2 characters")
//     .max(50, "Policy number must be at most 50 characters"),
//   allergies: z.string().optional(),
//   currentMedication: z.string().optional(),
//   familyMedicalHistory: z.string().optional(),
//   pastMedicalHistory: z.string().optional(),
//   identificationType: z.string().optional(),
//   identificationNumber: z.string().optional(),
//   identificationDocument: z.custom<File[]>().optional(),
//   treatmentConsent: z
//     .boolean()
//     .default(false)
//     .refine((value) => value === true, {
//       message: "You must consent to treatment in order to proceed",
//     }),
//   disclosureConsent: z
//     .boolean()
//     .default(false)
//     .refine((value) => value === true, {
//       message: "You must consent to disclosure in order to proceed",
//     }),
//   privacyConsent: z
//     .boolean()
//     .default(false)
//     .refine((value) => value === true, {
//       message: "You must consent to privacy in order to proceed",
//     }),
// });

// export const CreateAppointmentSchema = z.object({
//   primaryPhysician: z.string().min(2, "Select at least one doctor"),
//   schedule: z.coerce.date(),
//   reason: z
//     .string()
//     .min(2, "Reason must be at least 2 characters")
//     .max(500, "Reason must be at most 500 characters"),
//   note: z.string().optional(),
//   cancellationReason: z.string().optional(),
// });

// export const ScheduleAppointmentSchema = z.object({
//   primaryPhysician: z.string().min(2, "Select at least one doctor"),
//   schedule: z.coerce.date(),
//   reason: z.string().optional(),
//   note: z.string().optional(),
//   cancellationReason: z.string().optional(),
// });

// export const CancelAppointmentSchema = z.object({
//   primaryPhysician: z.string().min(2, "Select at least one doctor"),
//   schedule: z.coerce.date(),
//   reason: z.string().optional(),
//   note: z.string().optional(),
//   cancellationReason: z
//     .string()
//     .min(2, "Reason must be at least 2 characters")
//     .max(500, "Reason must be at most 500 characters"),
// });

// export function getAppointmentSchema(type: string) {
//   switch (type) {
//     case "create":
//       return CreateAppointmentSchema;
//     case "cancel":
//       return CancelAppointmentSchema;
//     default:
//       return ScheduleAppointmentSchema;
//   }
// }