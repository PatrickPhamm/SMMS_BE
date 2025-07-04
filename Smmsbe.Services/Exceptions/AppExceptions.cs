namespace Smmsbe.Services.Exceptions
{
    public static class AppExceptions
    {
        public static AppException BadRequest(string message)
        {
            return new BadRequestException("MSG00", message);
        }

        public static AppException NotFoundId()
        {
            return new NotFoundException("MSG01", "Không tìm thấy Id.");
        }

        public static AppException IdExists()
        {
            return new NotFoundException("MSG02", "Id đã tồn tại.");
        }

        public static AppException BadRequestStudentNumberExists()
        {
            return new BadRequestException("MSG03", "Mã số học sinh đã tồn tại");
        }

        public static AppException BadRequestEmailIsInvalid()
        {
            return new BadRequestException("MSG04", "Email người dùng không hợp lệ");
        }

        public static AppException BadRequestEmailIsExists()
        {
            return new BadRequestException("MSG05", "Email người dùng đã tồn tại");
        }

        public static AppException NotFoundAccount()
        {
            return new NotFoundException("MSG06", "Không tìm thấy tài khoản.");
        }

        public static AppException AcceptedForm()
        {
            return new NotFoundException("MSG07", "Đơn đã được đồng ý.");
        }

        public static AppException RejectedForm()
        {
            return new NotFoundException("MSG08", "Đơn đã được từ chối.");
        }

        public static AppException AccountNotActivated()
        {
            return new BadRequestException("MSG09", "Tài khoản chưa được kích hoạt.");
        }

        public static AppException ScheduleAlreadyExist()
        {
            return new BadRequestException("MSG10", "Ngày này đã có lịch. Vui lòng chọn ngày khác");
        }
    }
}
