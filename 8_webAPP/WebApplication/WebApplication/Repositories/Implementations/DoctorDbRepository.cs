using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WebApplication.DTO.Requests;
using WebApplication.DTO.Responses;
using WebApplication.Models;
using WebApplication.Repositories.Interfaces;
using WebApplication.Status;

namespace WebApplication.Repositories.Implementations
{
    public class DoctorDbRepository : IDoctorDbRepository
    {
        private readonly HospitalContext _context;

        public DoctorDbRepository(HospitalContext context)
        {
            _context = context;
        }

        public async Task<ICollection<DoctorResponseDTO>> GetDoctors()
        {
            var doctorsFromDb =  await _context.Doctor
                .Select(doctor => new DoctorResponseDTO
                {
                    IdDoctor = doctor.IdDoctor,
                    FirstName = doctor.FirstName,
                    LastName = doctor.LastName,
                    Email = doctor.Email
                }).ToListAsync();

            return doctorsFromDb;
        }

        public async Task<MyStatus> PostDoctor(DoctorRequestDTO doctor)
        {
            await _context.AddAsync(new Doctor
            {
                FirstName = doctor.FirstName,
                LastName = doctor.LastName,
                Email = doctor.Email
            });
        
            await _context.SaveChangesAsync();

            var newDoctorId = _context.Doctor.Max(d => d.IdDoctor);

            var answer = new MyStatus
            {
                Code = 200,
                Message = "Doctor added, ID = " + newDoctorId
            };

            return answer;
        }

        public async Task<MyStatus> PutDoctor(DoctorRequestDTO doctor, int idDoctor)
        {
            var code = 0;
            var message = "";
            
            var findDoctor = await _context.Doctor.SingleOrDefaultAsync(d => d.IdDoctor == idDoctor);
            
            if (findDoctor == null)
            {
                code = 404;
                message = "There is no such Doctor in the database";
            }
            else
            {
                findDoctor.FirstName = doctor.FirstName;
                findDoctor.LastName = doctor.LastName;
                findDoctor.Email = doctor.Email;
              
                await _context.SaveChangesAsync();

                code = 200;
                message = "Doctor with ID = " + idDoctor + " has been changed";
            }
            
            var answer = new MyStatus
            {
                Code = code,
                Message = message
            };

            return answer;
        }

        public async Task<MyStatus> DeleteDoctor(int idDoctor)
        {
            var code = 0;
            var message = "";
            
            var findDoctor = await _context.Doctor.SingleOrDefaultAsync(d => d.IdDoctor == idDoctor);
            var findPrescription = await _context.Prescription.AnyAsync(p => p.IdDoctor == idDoctor);
            
            if (findDoctor == null)
            {
                code = 404;
                message = "There is no such Doctor in the database";
            }
            else if(findPrescription == true)
            {
                code = 404;
                message = "Doctor is in the prescription table. Can't delete.";
            }
            else
            {
                _context.Remove(findDoctor);
                await _context.SaveChangesAsync();

                code = 200;
                message = "Doctor with ID = " + idDoctor + " has been deleted";
            }
            var answer = new MyStatus
            {
                Code = code,
                Message = message
            };

            return answer;
        }
        
        

    }
}