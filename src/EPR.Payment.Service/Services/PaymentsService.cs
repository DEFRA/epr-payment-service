﻿using AutoMapper;
using EPR.Payment.Service.Common.Data.Interfaces.Repositories;
using EPR.Payment.Service.Common.Dtos.Request;
using EPR.Payment.Service.Services.Interfaces;

namespace EPR.Payment.Service.Services
{
    public class PaymentsService : IPaymentsService
    {
        private readonly IPaymentsRepository _paymentRepository;
        private IMapper _mapper;
        public PaymentsService(IMapper mapper, IPaymentsRepository paymentRepository)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _paymentRepository = paymentRepository;
        }
        public async Task<Guid> InsertPaymentStatusAsync(PaymentStatusInsertRequestDto paymentStatusInsertRequest)
        {
            ValidatePaymentStatusInsertRequest(paymentStatusInsertRequest);
            var entity = _mapper.Map<Common.Data.DataModels.Payment>(paymentStatusInsertRequest);
            return await _paymentRepository.InsertPaymentStatusAsync(entity);
        }

        public async Task UpdatePaymentStatusAsync(Guid externalPaymentId, PaymentStatusUpdateRequestDto paymentStatusUpdateRequest)
        {
            ValidatePaymentStatusUpdateRequest(paymentStatusUpdateRequest);
            var entity = await _paymentRepository.GetPaymentByExternalPaymentIdAsync(externalPaymentId);
            entity = _mapper.Map(paymentStatusUpdateRequest, entity);
            await _paymentRepository.UpdatePaymentStatusAsync(entity);
        }

        private static void ValidatePaymentStatusInsertRequest(PaymentStatusInsertRequestDto paymentStatusInsertRequest)
        {
            if (paymentStatusInsertRequest == null)
                throw new ArgumentNullException(nameof(paymentStatusInsertRequest));

            if (string.IsNullOrEmpty(paymentStatusInsertRequest.UserId))
                throw new ArgumentException("User ID cannot be null or empty.", nameof(paymentStatusInsertRequest.UserId));
            if (string.IsNullOrEmpty(paymentStatusInsertRequest.OrganisationId))
                throw new ArgumentException("Organisation ID cannot be null or empty.", nameof(paymentStatusInsertRequest.OrganisationId));
            if (string.IsNullOrEmpty(paymentStatusInsertRequest.ReferenceNumber))
                throw new ArgumentException("Reference Number cannot be null or empty.", nameof(paymentStatusInsertRequest.ReferenceNumber));
        }
        private static void ValidatePaymentStatusUpdateRequest(PaymentStatusUpdateRequestDto paymentStatusUpdateRequest)
        {
            if (paymentStatusUpdateRequest == null)
                throw new ArgumentNullException(nameof(paymentStatusUpdateRequest));

            if (string.IsNullOrEmpty(paymentStatusUpdateRequest.UpdatedByUserId))
                throw new ArgumentException("User ID cannot be null or empty.", nameof(paymentStatusUpdateRequest.UpdatedByUserId));
            if (string.IsNullOrEmpty(paymentStatusUpdateRequest.UpdatedByOrganisationId))
                throw new ArgumentException("Organisation ID cannot be null or empty.", nameof(paymentStatusUpdateRequest.UpdatedByOrganisationId));
            if (string.IsNullOrEmpty(paymentStatusUpdateRequest.ReferenceNumber))
                throw new ArgumentException("Reference Number cannot be null or empty.", nameof(paymentStatusUpdateRequest.ReferenceNumber));
        }
        public async Task<int> GetPaymentStatusCount()
        {
            return await _paymentRepository.GetPaymentStatusCount();
        }

    }
}
