using AutoMapper;
using DeveloperStore.Application.DTOs;
using DeveloperStore.Domain.Entities;
using DeveloperStore.Domain.Interfaces;

namespace DeveloperStore.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _repository;
        private readonly IMapper _mapper;

        public UserService(IUserRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<List<UserResponse>> GetAllAsync()
        {
            var users = await _repository.GetAllAsync();
            return _mapper.Map<List<UserResponse>>(users);
        }

        public async Task<UserResponse?> GetByIdAsync(int id)
        {
            var user = await _repository.GetByIdAsync(id);
            return user is null ? null : _mapper.Map<UserResponse>(user);
        }

        public async Task<UserResponse> CreateAsync(UserRequest request)
        {
            var user = _mapper.Map<User>(request);
            var created = await _repository.AddAsync(user);
            return _mapper.Map<UserResponse>(created);
        }

        public async Task UpdateAsync(int id, UserRequest request)
        {
            var user = await _repository.GetByIdAsync(id) ?? throw new Exception("User not found");
            _mapper.Map(request, user);
            await _repository.UpdateAsync(user);
        }

        public async Task DeleteAsync(int id)
        {
            await _repository.DeleteAsync(id);
        }
    }
}
